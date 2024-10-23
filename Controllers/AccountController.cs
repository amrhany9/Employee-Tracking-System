using back_end.Data;
using back_end.DTOs;
using back_end.Entities;
using back_end.Interfaces;
using back_end.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(ApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("log-in")] // POST : api/account/login
        public ActionResult<string> Login(LoginDTO loginDTO)
        {
            var userAccount = _context.UsersAccounts.SingleOrDefault(x => x.UserName == loginDTO.UserName);

            if (userAccount == null)
            {
                return Unauthorized("Invalid username.");
            }

            if (userAccount.Password != loginDTO.Password)
            {
                return Unauthorized("Invalid password.");
            }

            var token = _tokenService.GenerateToken(userAccount);

            return Ok(new {
                Token = token,
                Role = userAccount.Role
            });
        }

        [HttpGet("log-out")]
        [Authorize]
        public ActionResult<string> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID could not be found.");
            }

            var userAccount = _context.UsersAccounts.SingleOrDefault(x => x.UserId == int.Parse(userId));

            if (userAccount == null)
            {
                return BadRequest("This User Account Is Not Found");
            }

            if (userAccount.IsCheckedIn)
            {
                return BadRequest("You Must Check Out Before Logging Out");
            }

            return Ok();
        }

        [HttpPost("register")] // POST : api/account/register
        [Authorize(Roles = "Admin")]
        public ActionResult<string> Register(RegisterDTO registerDTO)
        {
            var currentAdminUserId = User.Claims.FirstOrDefault(c => c.Type == "userId").Value;

            if (currentAdminUserId == null)
            {
                return Unauthorized("Current user is not authenticated.");
            }

            var adminUser = _context.UsersAccounts.FirstOrDefault(u => u.UserId == int.Parse(currentAdminUserId));

            if (adminUser == null || adminUser.Role != "Admin")
            {
                return Forbid("Only admins can register new users.");
            }

            if (_context.UsersAccounts.Any(u => u.UserName == registerDTO.UserName))
            {
                return BadRequest("Username is already taken.");
            }

            var userAccount = new UserAccount
            {
                UserId = registerDTO.UserId,
                UserName = registerDTO.UserName,
                Password = registerDTO.Password,
                Role = registerDTO.Role
            };

            _context.UsersAccounts.Add(userAccount);
            _context.SaveChanges();

            return Ok("User Account Created Succesfully.");
        }
    }
}
