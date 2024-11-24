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

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private ILocationService _locationService;
        private IAttendanceMediator _attendanceMediator;

        public AttendanceController(ILocationService locationService, IAttendanceMediator attendanceMediator)
        {
            _locationService = locationService;
            _attendanceMediator = attendanceMediator;
        }

        [HttpGet("attendance-log")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<Attendance>> GetAttendanceLog()
        {
            var attendanceLog = _context.Attendances.ToList();
            return Ok(attendanceLog);
        }

        [HttpGet("user-attendance-log")]
        [Authorize]
        public ActionResult<List<Attendance>> GetUserAttendanceLog()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID could not be found.");
            }

            var userAttendanceLog = 

            return Ok(userAttendanceLog);
        }

        [HttpPost("check-in")]
        [Authorize]
        public ActionResult<Attendance> CheckIn(CheckInOutDTO checkInOutDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID could not be found.");
            }

            var userAccount = _context.Accounts.FirstOrDefault(u => u.UserId == int.Parse(userId)); // Find user by ID

            if (userAccount == null)
            {
                return BadRequest("This User Account Is Not Found");
            }

            if (userAccount.IsCheckedIn)
            {
                return BadRequest("This User Account Is Already Checked In");
            }

            bool isInCompanyArea = _locationService.IsWithinCompanyArea(checkInOutDTO.Latitude, checkInOutDTO.Longitude);

            if (!isInCompanyArea)
            {
                return BadRequest("You Must Be Within The Company Area To Check In");
            }

            var userAttendance = new Attendance
            {
                UserId = int.Parse(userId),
                VerifyMode = 2,
                CheckType = 0,
                CheckDate = checkInOutDTO.CheckDate,
            };

            _context.Attendances.Add(userAttendance);
            userAccount.IsCheckedIn = true;
            _context.Accounts.Update(userAccount);
            _context.SaveChanges();

            return Ok(userAttendance);
        }

        [HttpPost("check-out")]
        [Authorize]
        public async Task<ActionResult<Attendance>> CheckOut(CheckInOutDTO checkInOutDTO)
        {
            // Get the User ID from the claims (the token)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID could not be found.");
            }

            var userAccount = _context.Accounts.FirstOrDefault(u => u.UserId == int.Parse(userId));

            if (userAccount == null)
            {
                return BadRequest("This User Account Is Not Found");
            }

            if (!userAccount.IsCheckedIn)
            {
                return BadRequest("This User Account Is Not Checked In");
            }

            var userAttendance = new Attendance
            {
                UserId = int.Parse(userId),
                VerifyMode = 2,
                CheckType = 1,
                CheckDate = checkInOutDTO.CheckDate,
            };

            _context.Attendances.Add(userAttendance);
            userAccount.IsCheckedIn = false;
            _context.Accounts.Update(userAccount);
            await _context.SaveChangesAsync();

            return Ok(userAttendance);
        }
    }
}
