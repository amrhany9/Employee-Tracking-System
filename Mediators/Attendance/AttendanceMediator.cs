using AutoMapper;
using back_end.Constants.Enums;
using back_end.Data;
using back_end.Models;
using back_end.Repositories;
using back_end.Services.Attendance;
using back_end.Services.ZKEM_Machine;
using Microsoft.EntityFrameworkCore;

namespace back_end.Mediators.Attendance
{
    public class AttendanceMediator : IAttendanceMediator
    {
        private ApplicationDbContext _context;
        private IAttendanceService _attendanceService;
        private IRepository<Models.User> _userRepository;
        private IRepository<Models.AttArchive> _attArchiveRepository;
        private IRepository<Models.AttMachine> _attMachineRepository;
        private IMachineService _machineService;
        private IMapper _mapper;

        public AttendanceMediator(ApplicationDbContext context, IAttendanceService attendanceService, IRepository<Models.User> userRepository, IRepository<Models.AttArchive> attArchiveRepository, IRepository<Models.AttMachine> attMachineRepository, IMachineService machineService, IMapper mapper)
        {
            _context = context;
            _attendanceService = attendanceService;
            _userRepository = userRepository;
            _attArchiveRepository = attArchiveRepository;
            _attMachineRepository = attMachineRepository;
            _machineService = machineService;
            _mapper = mapper;
        }

        public void SyncDailyLog()
        {
            var machine = _attMachineRepository.GetByFilter(x => x.MachineCode == 1).First();
            _machineService.setDeviceNetwork(machine.MachineIP, machine.MachinePort);
            //if (_machineService.isConnected())
            //{
            //    var MachineLog = _machineService.GetDailyAttendanceRecords();
            //    var DbLog = _attendanceService.GetDailyLog().ToList();
            //    var UniqueLog = MachineLog.Except(DbLog).ToList();

            //    foreach (var attendance in UniqueLog)
            //    {
            //        var user = _userRepository.GetById(attendance.UserId).First();

            //        if (user != null)
            //        {
            //            switch (attendance.CheckType)
            //            {
            //                case CheckType.CheckIn:
            //                    user.IsCheckedIn = true;
            //                    break;

            //                case CheckType.CheckOut:
            //                    user.IsCheckedIn = false;
            //                    break;

            //                default:
            //                    Console.WriteLine($"Unknown CheckType for attendance ID {attendance.Id}");
            //                    break;
            //            }
            //        }
            //    }

            //    using (var transaction = _context.Database.BeginTransaction())
            //    {
            //        try
            //        {
            //            _attendanceService.AddLog(UniqueLog);
            //            _attendanceService.SaveChanges();
            //            _userRepository.SaveChanges();
            //            transaction.Commit();
            //        }
            //        catch (Exception ex)
            //        {
            //            transaction.Rollback();
            //            throw new Exception("Error during sync operation: " + ex.Message);
            //        }
            //    }
            //}
        }

        public void DeleteDailyLog()
        {
            _attendanceService.DeleteLog(_attendanceService.GetDailyLog().ToList());
            _attendanceService.SaveChanges();
        }

        public void ArchiveWeekLogs()
        {
            var LogsToArchive = _attendanceService.GetLastWeekLog().ToList();
            var archiveLogs = _mapper.Map<IEnumerable<Models.AttArchive>>(LogsToArchive);
            _attendanceService.HardDeleteLog(LogsToArchive);
            _attArchiveRepository.AddRange(archiveLogs);
            _attendanceService.SaveChanges();
            _attArchiveRepository.SaveChanges();
        }
    }
}
