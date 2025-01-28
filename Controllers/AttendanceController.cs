using back_end.Data;
using back_end.DTOs;
using back_end.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using back_end.Services;
using back_end.Services.Location;
using back_end.Services.ZKEM_Machine;
using back_end.Services.Attendance;
using back_end.Repositories;
using back_end.Constants.Enums;

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private ILocationService _locationService;
        private IAttendanceService _attendanceService;
        private IZKMachineService _zkMachineService;
        private IRepository<Employee> _employeeRepository;

        public AttendanceController(ILocationService locationService, IZKMachineService zkMachineService, IAttendanceService attendanceService, IRepository<Employee> employeeRepository)
        {
            _locationService = locationService;
            _zkMachineService = zkMachineService;
            _attendanceService = attendanceService;
            _employeeRepository = employeeRepository;
        }

        [HttpGet("Get-Daily-Log")]
        public ActionResult<IEnumerable<Attendance>> GetDailyAttendanceLog()
        {
            return Ok(_attendanceService.GetDailyLog().ToList());
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

        [HttpPost("check-out")]
        [Authorize]
        public ActionResult<Attendance> CheckOut(LocationDTO locationDTO)
        {
            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (employeeId == null)
            {
                return Unauthorized("Employee ID could not be found.");
            }

            var employee = _employeeRepository.GetByFilter(x => x.employeeId == int.Parse(employeeId)).FirstOrDefault();

            if (!employee.isCheckedIn)
            {
                return BadRequest("Account is not checked in");
            }

            var userAttendance = new Attendance
            {
                machineCode = 0,
                employeeId = int.Parse(employeeId),
                verifyMode = VerifyMode.Website,
                checkType = CheckType.CheckOut,
                checkDate = DateTime.Now,
                latitude = (decimal)locationDTO.Latitude,
                longitude = (decimal)locationDTO.Longitude
            };

            _attendanceService.AddAttendance(userAttendance);
            employee.isCheckedIn = false;
            _employeeRepository.Update(employee);
            _employeeRepository.SaveChanges();
            _attendanceService.SaveChanges();

            return Ok(userAttendance);
        }
    }
}
