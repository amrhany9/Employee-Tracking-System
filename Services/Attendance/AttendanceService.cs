using back_end.Models;
using back_end.Repositories;

namespace back_end.Services.Attendance
{
    public class AttendanceService : IAttendanceService
    {
        private IRepository<Models.Attendance> _attendanceRepository;

        public AttendanceService(IRepository<Models.Attendance> attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public void AddAttendance(Models.Attendance attendance)
        {
            _attendanceRepository.Add(attendance);
        }

        public IQueryable<Models.Attendance> GetLogByMachineCode(int machineCode)
        {
            return _attendanceRepository.GetByFilter(x => x.MachineCode == machineCode);
        }

        public IQueryable<Models.Attendance> GetDailyLog()
        {
            return _attendanceRepository.GetByFilter(x => x.CheckDate.Date == DateTime.Today);
        }

        public IQueryable<Models.Attendance> GetDailyLogByMachineCode(int machineCode)
        {
            return _attendanceRepository.GetByFilter(x => x.CheckDate.Date == DateTime.Today && x.MachineCode == machineCode);
        }

        public void AddLog(IEnumerable<Models.Attendance> Log)
        {
            _attendanceRepository.AddRange(Log);
        }

        public void DeleteLog(IEnumerable<Models.Attendance> Log)
        {
            _attendanceRepository.DeleteRange(Log);
        }

        public void SaveChanges()
        {
            _attendanceRepository.SaveChanges();
        }
    }
}
