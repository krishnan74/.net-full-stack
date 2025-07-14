using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using QuizupAPI.Models;
using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models.DTOs.Response;
using Serilog;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using ILogger = Serilog.ILogger;

namespace QuizupAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly QuizContext _context;
        private readonly IPerformanceMonitoringService _performanceService;
        private readonly ILogger _logger;

        public HealthController(QuizContext context, IPerformanceMonitoringService performanceService)
        {
            _context = context;
            _performanceService = performanceService;
            _logger = Log.ForContext<HealthController>();
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetHealth()
        {
            var healthStatus = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                Database = await CheckDatabaseHealth(),
                Uptime = GetUptime()
            };

            _logger.Information(
                "Health check completed - Status: {Status}, Database: {DatabaseStatus}",
                healthStatus.Status,
                healthStatus.Database.Status
            );

            return Ok(ApiResponse<object>.SuccessResponse(healthStatus, "Health check completed successfully"));
        }


        [HttpGet("metrics")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [Authorize(Roles = "Admin")]
        public IActionResult GetMetrics()
        {
            var process = Process.GetCurrentProcess();
            var metrics = new
            {
                Timestamp = DateTime.UtcNow,
                Memory = new
                {
                    WorkingSetMB = process.WorkingSet64 / 1024 / 1024
                },
                Threads = process.Threads.Count,
                Uptime = GetUptime()
            };

            _performanceService.LogBasicMetrics();

            _logger.Information(
                "Basic metrics collected - Memory: {WorkingSetMB}MB, Threads: {ThreadCount}",
                metrics.Memory.WorkingSetMB,
                metrics.Threads
            );

            return Ok(ApiResponse<object>.SuccessResponse(metrics, "Metrics retrieved successfully"));
        }

        [HttpGet("database")]
        [ProducesResponseType(typeof(ApiResponse<DatabaseHealth>), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDatabaseHealth()
        {
            var dbHealth = await CheckDatabaseHealth();

            _logger.Information(
                "Database health check - Status: {Status}, ResponseTime: {ResponseTime}ms",
                dbHealth.Status,
                dbHealth.ResponseTime
            );

            return Ok(ApiResponse<DatabaseHealth>.SuccessResponse(dbHealth, "Database health check completed successfully"));
        }

        private async Task<DatabaseHealth> CheckDatabaseHealth()
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                // Test database connectivity
                await _context.Database.CanConnectAsync();
                stopwatch.Stop();

                var responseTime = stopwatch.ElapsedMilliseconds;
                
                _performanceService.TrackDatabaseOperation("HEALTH_CHECK", responseTime);

                return new DatabaseHealth
                {
                    Status = "Healthy",
                    ResponseTime = responseTime,
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var responseTime = stopwatch.ElapsedMilliseconds;
                
                _performanceService.TrackDatabaseOperation("HEALTH_CHECK_FAILED", responseTime);
                
                _logger.Error(ex, "Database health check failed");

                return new DatabaseHealth
                {
                    Status = "Unhealthy",
                    ResponseTime = responseTime,
                    Error = ex.Message,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        private object GetUptime()
        {
            var process = Process.GetCurrentProcess();
            var uptime = DateTime.UtcNow - process.StartTime.ToUniversalTime();

            return new
            {
                TotalSeconds = uptime.TotalSeconds,
                Formatted = $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s"
            };
        }
    }
} 