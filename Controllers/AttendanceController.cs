using back_end.DTOs;
using back_end.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using back_end.Services.Attendance;
using back_end.Repositories;
using back_end.Constants.Enums;

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private IAttendanceService _attendanceService;
        private IRepository<Employee> _employeeRepository;

        public AttendanceController(IAttendanceService attendanceService, IRepository<Employee> employeeRepository)
        {
            _attendanceService = attendanceService;
            _employeeRepository = employeeRepository;
        }

        [HttpGet("Daily/All")]
        public ActionResult GetDailyAttendanceLog()
        {
            var dailyLog = _attendanceService.GetDailyLog().ToList();

            return Ok(dailyLog);
        }

        [HttpGet("Daily/{machineCode}")]
        public ActionResult GetDailyAttendanceLogByMachineCode(int machineCode)
        {
            var machineLog = _attendanceService.GetDailyLogByMachineCode(machineCode).ToList();

            return Ok(machineLog);
        }

        [HttpGet("{machineCode}")]
        public ActionResult GetAttendanceLogByMachineCode(int machineCode)
        {
            var machineLog = _attendanceService.GetLogByMachineCode(machineCode).ToList();

            return Ok(machineLog);
        }

        //[HttpPost("check-in")]
        //[Authorize]
        //public ActionResult<Attendance> CheckIn(LocationDTO locationDTO)
        //{
        //    var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (employeeId == null)
        //    {
        //        return Unauthorized("Employee ID not found.");
        //    }

        //    var employee = _employeeRepository.GetByFilter(x => x.employeeId == int.Parse(employeeId)).FirstOrDefault();

        //    if (employee.isCheckedIn)
        //    {
        //        return BadRequest("This User Account Is Already Checked In");
        //    }

        //    var userAttendance = new Attendance
        //    {
        //        machineCode = 0,
        //        employeeId = int.Parse(employeeId),
        //        verifyMode = VerifyMode.Website,
        //        checkType = CheckType.CheckIn,
        //        checkDate = DateTime.Now,
        //        latitude = (decimal)locationDTO.Latitude,
        //        longitude = (decimal)locationDTO.Longitude
        //    };

        //    _attendanceService.AddAttendance(userAttendance);
        //    employee.isCheckedIn = true;
        //    _employeeRepository.Update(employee);
        //    _employeeRepository.SaveChanges();
        //    _attendanceService.SaveChanges();

        //    return Ok(userAttendance);
        //}

        [HttpPost("CheckOut")]
        public ActionResult CheckOut(LocationDTO locationDTO)
        {
            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (employeeId == null)
            {
                return Unauthorized("Employee ID could not be found.");
            }

            var employee = _employeeRepository.GetByFilter(x => x.Id == int.Parse(employeeId)).FirstOrDefault();

            if (!employee.IsCheckedIn)
            {
                return BadRequest("Account is not checked in");
            }

            var userAttendance = new Attendance
            {
                MachineCode = 0,
                EmployeeId = int.Parse(employeeId),
                VerifyMode = VerifyMode.Website,
                CheckType = CheckType.CheckOut,
                CheckDate = DateTime.Now,
                Latitude = (decimal)locationDTO.Latitude,
                Longitude = (decimal)locationDTO.Longitude
            };

            _attendanceService.AddAttendance(userAttendance);
            employee.IsCheckedIn = false;
            _employeeRepository.Update(employee);
            _employeeRepository.SaveChanges();
            _attendanceService.SaveChanges();

            return Ok(userAttendance);
        }
    }
}
