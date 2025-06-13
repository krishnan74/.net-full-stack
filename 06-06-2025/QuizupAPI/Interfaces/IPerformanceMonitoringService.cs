namespace QuizupAPI.Interfaces
{
    public interface IPerformanceMonitoringService
    {
        void TrackDatabaseOperation(string operation, long elapsedMs);
        void LogBasicMetrics();
    }
}