using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;
using System.Text.Json;
using System.Security.Claims;

namespace QuizupAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _environment;

        public ExceptionHandlingMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            _next = next;
            _environment = environment;
            _logger = Log.ForContext<ExceptionHandlingMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var username = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous";
            var role = context.User?.FindFirst(ClaimTypes.Role)?.Value ?? "anonymous";
            var requestPath = context.Request.Path;
            var requestMethod = context.Request.Method;

            // Logging the exception
            _logger.Error(
                exception,
                "Unhandled exception occurred. Request: {Method} {Path} by {Username} (Role: {Role}). " +
                "Exception: {ExceptionType}, Message: {ExceptionMessage}",
                requestMethod,
                requestPath,
                username,
                role,
                exception.GetType().Name,
                exception.Message
            );

            // Logging stack trace
            
            _logger.Error(
                "Stack trace: {StackTrace}",
                exception.StackTrace
            );

            context.Response.StatusCode = GetStatusCode(exception);
            context.Response.ContentType = "application/json";

            
            var responseWithStackTrace = new
            {
                exception.Message,
                context.Response.StatusCode,
                DateTime.UtcNow,
                StackTrace = exception.StackTrace
            };

            var jsonResponse = JsonSerializer.Serialize(responseWithStackTrace, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await context.Response.WriteAsync(jsonResponse);
            
            
        }

        private static int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                InvalidOperationException => (int)HttpStatusCode.Conflict,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }
    }
} 