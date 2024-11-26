using AutoMapper;
using back_end.Repositories;
using back_end.Services.Attendance;
using back_end.Services.ZKEM_Machine;

namespace back_end.Mediators.Attendance
{
    public class AttendanceMediator : IAttendanceMediator
    {
        private IAttendanceService _attendanceService;
        private IRepository<Models.AttendanceArchive> _attendanceArchiveRepository;
        private IMachineService _machineService;
        private IMapper _mapper;

        public AttendanceMediator(IAttendanceService attendanceService, IRepository<Models.AttendanceArchive> attendanceArchiveRepository, IMachineService machineService, IMapper mapper)
        {
            _attendanceService = attendanceService;
            _attendanceArchiveRepository = attendanceArchiveRepository;
            _machineService = machineService;
            _mapper = mapper;
        }

        public void SyncDailyLogToDatabase()
        {
            if (_machineService.isConnected())
            {
                IEnumerable<Models.Attendance> DailyLog = _machineService.GetDailyAttendanceRecords();
                HandleDuplicatesInDatabase(DailyLog);
            }
        }

        private void HandleDuplicatesInDatabase(IEnumerable<Models.Attendance> dailyLog)
        {
            IEnumerable<Models.Attendance> Log = _attendanceService.GetDailyLog();
            IEnumerable<Models.Attendance> UniqueLog = new List<Models.Attendance>();
            if (Log.Any())
            {
                foreach (var attendance in Log)
                {
                    foreach (var dailyAttendance in dailyLog)
                    {
                        if (dailyAttendance != attendance)
                        {
                            UniqueLog.Append(dailyAttendance);
                        }
                    }
                }
            }
            _attendanceService.AddLog(Log);
            _attendanceService.SaveChanges();
        }

        public void MarkDailyLogAsDeleted()
        {
            _attendanceService.DeleteLog(_attendanceService.GetDailyLog());
        }

        public void ArchiveWeekLogs()
        {
            IEnumerable<Models.Attendance> LogsToArchive = _attendanceService.GetLastWeekLog();
            IEnumerable<Models.AttendanceArchive> archiveLogs = _mapper.Map<IEnumerable<Models.AttendanceArchive>>(LogsToArchive);
            _attendanceService.HardDeleteLog(LogsToArchive);
            _attendanceArchiveRepository.AddRange(archiveLogs);
            _attendanceService.SaveChanges();
            _attendanceArchiveRepository.SaveChanges();
        }
    }
}
