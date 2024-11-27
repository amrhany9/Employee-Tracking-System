namespace back_end.Services.Attendance
{
    public interface IAttendanceService
    {
        void AddAttendance(Models.Attendance attendance);
        IEnumerable<Models.Attendance> GetLastWeekLog();
        IEnumerable<Models.Attendance> GetDailyLog();
        void AddLog(IEnumerable<Models.Attendance> Log);
        void DeleteLog(IEnumerable<Models.Attendance> Log);
        void HardDeleteLog(IEnumerable<Models.Attendance> Log);
        void SaveChanges();
    }
}
