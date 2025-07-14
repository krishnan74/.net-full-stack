using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using System.Data.Common;
using System.Diagnostics;
using ILogger = Serilog.ILogger;
using System.Threading;

namespace QuizupAPI.Interceptors
{

    public class DatabasePerformanceInterceptor : DbCommandInterceptor
    {
        private readonly ILogger _logger;
        private const int SLOW_QUERY_THRESHOLD = 1000; 

        public DatabasePerformanceInterceptor()
        {
            _logger = Log.ForContext<DatabasePerformanceInterceptor>();
        }

        public override async ValueTask<DbDataReader> ReaderExecutedAsync(
            DbCommand command,
            CommandExecutedEventData eventData,
            DbDataReader result,
            CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();
            var result2 = await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
            stopwatch.Stop();

            var timeTakenInMs = stopwatch.ElapsedMilliseconds;

            // Logging slow queries 
            if (timeTakenInMs > SLOW_QUERY_THRESHOLD)
            {
                _logger.Warning(
                    "Slow database query detected: {QueryType} took {timeTakenInMs}ms",
                    GetQueryType(command.CommandText),
                    timeTakenInMs
                );
            }

            return result2;
        }

        public override async ValueTask<int> NonQueryExecutedAsync(
            DbCommand command,
            CommandExecutedEventData eventData,
            int rowsAffected,
            CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();
            var result2 = await base.NonQueryExecutedAsync(command, eventData, rowsAffected, cancellationToken);
            stopwatch.Stop();

            var timeTakenInMs = stopwatch.ElapsedMilliseconds;

            // Logging slow operations 
            if (timeTakenInMs > SLOW_QUERY_THRESHOLD)
            {
                _logger.Warning(
                    "Slow database operation detected: {OperationType} took {timeTakenInMs}ms",
                    GetQueryType(command.CommandText),
                    timeTakenInMs
                );
            }

            return result2;
        }
        private static string GetQueryType(string commandText)
        {
            var command = commandText.Trim().ToUpperInvariant();
            
            if (command.StartsWith("SELECT"))
                return "SELECT";
            if (command.StartsWith("INSERT"))
                return "INSERT";
            if (command.StartsWith("UPDATE"))
                return "UPDATE";
            if (command.StartsWith("DELETE"))
                return "DELETE";
            
            return "UNKNOWN";
        }
    }
} 