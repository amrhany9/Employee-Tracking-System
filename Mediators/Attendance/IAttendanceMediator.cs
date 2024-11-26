namespace back_end.Mediators.Attendance
{
    public interface IAttendanceMediator
    {
        void SyncDailyLogToDatabase();
        void MarkDailyLogAsDeleted();
        void ArchiveWeekLogs();
    }
}
