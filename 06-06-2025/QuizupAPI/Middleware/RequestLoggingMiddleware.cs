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
                $"HTTP {context.Request.Method} {context.Request.Path} started by {username} (Role: {role}) at {requestTime}"
            );

            try
            {
                await _next(context);

                stopwatch.Stop();

                // Logging successful response
                _logger.Information(
                    $"HTTP {context.Request.Method} {context.Request.Path} completed by {username} (Role: {role}) in {stopwatch.ElapsedMilliseconds}ms with status {context.Response.StatusCode}"
                );

                // Logging slow request warning
                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    _logger.Warning(
                        $"Slow request detected: HTTP {context.Request.Method} {context.Request.Path} took {stopwatch.ElapsedMilliseconds}ms by {username} (Role: {role})"
                    );
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                // Logging error 
                _logger.Error(
                    ex,
                    $"HTTP {context.Request.Method} {context.Request.Path} failed for {username} (Role: {role}) after {stopwatch.ElapsedMilliseconds}ms. Error: {ex.Message}"
                );

                throw;
            }
        }
    }
} 