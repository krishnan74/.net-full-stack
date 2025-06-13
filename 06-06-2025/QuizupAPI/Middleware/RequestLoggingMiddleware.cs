using Microsoft.AspNetCore.Http;
using Serilog;
using System.Diagnostics;
using System.Security.Claims;
using ILogger = Serilog.ILogger;


namespace QuizupAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = Log.ForContext<RequestLoggingMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestTime = DateTime.UtcNow;
            var username = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous";
            var role = context.User?.FindFirst(ClaimTypes.Role)?.Value ?? "anonymous";

            // Log request start
            _logger.Information(
                "HTTP {Method} {Path} started by {Username} (Role: {Role}) at {RequestTime}",
                context.Request.Method,
                context.Request.Path,
                username,
                role,
                requestTime
            );

            try
            {
                await _next(context);

                stopwatch.Stop();

                // Logging successful response
                _logger.Information(
                    "HTTP {Method} {Path} completed by {Username} (Role: {Role}) in {ElapsedMs}ms with status {StatusCode}",
                    context.Request.Method,
                    context.Request.Path,
                    username,
                    role,
                    stopwatch.ElapsedMilliseconds,
                    context.Response.StatusCode
                );

                // Logging slow request warning
                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    _logger.Warning(
                        "Slow request detected: HTTP {Method} {Path} took {ElapsedMs}ms by {Username}",
                        context.Request.Method,
                        context.Request.Path,
                        stopwatch.ElapsedMilliseconds,
                        username
                    );
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                // Logging error 
                _logger.Error(
                    ex,
                    "HTTP {Method} {Path} failed for {Username} (Role: {Role}) after {ElapsedMs}ms. Error: {ErrorMessage}",
                    context.Request.Method,
                    context.Request.Path,
                    username,
                    role,
                    stopwatch.ElapsedMilliseconds,
                    ex.Message
                );

                throw;
            }
        }
    }
} 