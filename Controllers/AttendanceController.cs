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
using back_end.Mediators.Attendance;
using back_end.Services.ZKEM_Machine;
using back_end.Services.Attendance;
using back_end.Repositories;
using back_end.Constants.Enums;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private ILocationService _locationService;
        private IAttendanceMediator _attendanceMediator;
        private IAttendanceService _attendanceService;
        private IMachineService _machineService;
        private IRepository<Account> _accountRepository;

        public AttendanceController(ILocationService locationService, IAttendanceMediator attendanceMediator, IMachineService machineService, IAttendanceService attendanceService, IRepository<Account> accountRepository)
        {
            _locationService = locationService;
            _attendanceMediator = attendanceMediator;
            _machineService = machineService;
            _attendanceService = attendanceService;
            _accountRepository = accountRepository;
        }

        [HttpPost("check-in")]
        [Authorize]
        public ActionResult<Attendance> CheckIn(LocationDTO locationDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID could not be found.");
            }

            var Account = _accountRepository.GetByFilter(x => x.UserId == int.Parse(userId)).FirstOrDefault(); // Find user by ID

            if (Account == null)
            {
                return BadRequest("This User Account Is Not Found");
            }

            if (Account.IsCheckedIn)
            {
                return BadRequest("This User Account Is Already Checked In");
            }

            if (!_locationService.IsLocationSet())
            {
                return BadRequest("Company Location Is Not Set, Wait For Admin To Proceed");
            }

            bool isInCompanyArea = _locationService.IsWithinCompanyArea(locationDTO.Latitude, locationDTO.Longitude);

            if (!isInCompanyArea)
            {
                return BadRequest("You Must Be Within The Company Area To Check In");
            }

            var userAttendance = new Attendance
            {
                UserId = int.Parse(userId),
                VerifyMode = VerifyMode.Website,
                CheckType = CheckType.CheckIn,
                CheckDate = DateTime.Now,
            };

            _attendanceService.AddAttendance(userAttendance);
            Account.IsCheckedIn = true;
            _accountRepository.Update(Account);
            _accountRepository.SaveChanges();
            _attendanceService.SaveChanges();

            return Ok(userAttendance);
        }

        [HttpPost("check-out")]
        [Authorize]
        public ActionResult<Attendance> CheckOut(LocationDTO locationDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID could not be found.");
            }

            var Account = _accountRepository.GetByFilter(x => x.UserId == int.Parse(userId)).FirstOrDefault();

            if (Account == null)
            {
                return BadRequest("This User Account Is Not Found");
            }

            if (!Account.IsCheckedIn)
            {
                return BadRequest("This User Account Is Not Checked In");
            }

            var userAttendance = new Attendance
            {
                UserId = int.Parse(userId),
                VerifyMode = VerifyMode.Website,
                CheckType = CheckType.CheckOut,
                CheckDate = DateTime.Now,
            };

            _attendanceService.AddAttendance(userAttendance);
            Account.IsCheckedIn = false;
            _accountRepository.Update(Account);
            _accountRepository.SaveChanges();
            _attendanceService.SaveChanges();

            return Ok(userAttendance);
        }
    }
}
