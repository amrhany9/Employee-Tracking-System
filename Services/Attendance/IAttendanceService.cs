namespace back_end.Services.Attendance
{
    public interface IAttendanceService
    {
        void AddAttendance(Models.Attendance attendance);
        IQueryable<Models.Attendance> GetDailyLog();
        void AddLog(IEnumerable<Models.Attendance> Log);
        void DeleteLog(IEnumerable<Models.Attendance> Log);
        void SaveChanges();
    }
}
