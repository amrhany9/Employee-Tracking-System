namespace back_end.Mediators.Attendance
{
    public interface IAttendanceMediator
    {
        void SyncDailyLog();
        void DeleteDailyLog();
        void ArchiveWeekLogs();
    }
}
