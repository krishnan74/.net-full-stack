using Serilog;
using System.Diagnostics;
using QuizupAPI.Interfaces;

namespace QuizupAPI.Services
{
    public class PerformanceMonitoringService : IPerformanceMonitoringService
    {
        private readonly ILogger _logger;

        public PerformanceMonitoringService()
        {
            _logger = Log.ForContext<PerformanceMonitoringService>();
        }

        public void TrackDatabaseOperation(string operation, long elapsedMs)
        {
            // Logging slow database operations
            if (elapsedMs > 1000)
            {
                _logger.Warning(
                    "Slow database operation detected: {Operation} took {ElapsedMs}ms",
                    operation,
                    elapsedMs
                );
            }
        }

        public void LogBasicMetrics()
        {
            try
            {
                var process = Process.GetCurrentProcess();
                var workingSetMB = process.WorkingSet64 / 1024 / 1024;
                var threadCount = process.Threads.Count;

                _logger.Information(
                    "Basic system metrics - Memory: {WorkingSetMB}MB, Threads: {ThreadCount}",
                    workingSetMB,
                    threadCount
                );

                // Logging warning for high memory usage
                if (workingSetMB > 500)
                {
                    _logger.Warning(
                        "High memory usage detected: {WorkingSetMB}MB",
                        workingSetMB
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while logging basic metrics");
            }
        }
    }
} 