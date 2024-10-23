using back_end.Data;
using back_end.DTOs;
using back_end.Entities;
using back_end.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly ILocationService _locationService;
        private readonly IAttMachineService _attMachineService;

        public AttendanceController(ApplicationDbContext context, ITokenService tokenService, ILocationService locationService, IAttMachineService attMachineService)
        {
            _context = context;
            _tokenService = tokenService;
            _locationService = locationService;
            _attMachineService = attMachineService;
        }

        [HttpGet("attendance-log")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserAttendanceRecord>>> GetAttendanceLog()
        {
            var attendanceLog = await _context.UsersAttendanceRecords.ToListAsync();
            return Ok(attendanceLog);
        }

        [HttpGet("user-attendance-log")]
        [Authorize]
        public async Task<ActionResult<List<UserAttendanceRecord>>> GetUserAttendanceLog()
        {
            // Get the User ID from the claims (the token)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID could not be found.");
            }

            var userAttendanceLog = await _context.UsersAttendanceRecords
                .Where(r => r.UserId == int.Parse(userId))
                .ToListAsync();

            return Ok(userAttendanceLog);
        }

        [HttpPost("check-in")]
        [Authorize]
        public async Task<ActionResult<UserAttendanceRecord>> CheckIn(CheckInOutDTO checkInOutDTO)
        {
            // Get the User ID from the claims (the token)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID could not be found.");
            }

            var userAccount = await _context.UsersAccounts.FirstOrDefaultAsync(u => u.UserId == int.Parse(userId)); // Find user by ID

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

            var attendanceRecord = new UserAttendanceRecord
            {
                UserId = int.Parse(userId),
                VerifyMode = 2,
                CheckType = 0,
                CheckDate = checkInOutDTO.CheckDate,
            };

            _context.UsersAttendanceRecords.Add(attendanceRecord);
            userAccount.IsCheckedIn = true;
            _context.UsersAccounts.Update(userAccount);
            await _context.SaveChangesAsync();

            return Ok(attendanceRecord);
        }

        [HttpPost("check-out")]
        [Authorize]
        public async Task<ActionResult<UserAttendanceRecord>> CheckOut(CheckInOutDTO checkInOutDTO)
        {
            // Get the User ID from the claims (the token)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID could not be found.");
            }

            var userAccount = await _context.UsersAccounts.FirstOrDefaultAsync(u => u.UserId == int.Parse(userId));

            if (userAccount == null)
            {
                return BadRequest("This User Account Is Not Found");
            }

            if (!userAccount.IsCheckedIn)
            {
                return BadRequest("This User Account Is Not Checked In");
            }

            var attendanceRecord = new UserAttendanceRecord
            {
                UserId = int.Parse(userId),
                VerifyMode = 2,
                CheckType = 1,
                CheckDate = checkInOutDTO.CheckDate,
            };

            _context.UsersAttendanceRecords.Add(attendanceRecord);
            userAccount.IsCheckedIn = false;
            _context.UsersAccounts.Update(userAccount);
            await _context.SaveChangesAsync();

            return Ok(attendanceRecord);
        }
    }
}
