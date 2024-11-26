namespace back_end.Services.Attendance
{
    public interface IAttendanceService
    {
        IEnumerable<Models.Attendance> GetLastWeekLog();
        IEnumerable<Models.Attendance> GetDailyLog();
        void AddLog(IEnumerable<Models.Attendance> Log);
        void DeleteLog(IEnumerable<Models.Attendance> Log);
        void HardDeleteLog(IEnumerable<Models.Attendance> Log);
        void SaveChanges();
    }
}
