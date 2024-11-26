using back_end.Data;
using back_end.DTOs;
using back_end.Models;
using back_end.Repositories;
using back_end.Services.Token;
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
        private ITokenService _tokenService;
        private IRepository<Account> _accountRepository;

        public AccountController(ITokenService tokenService, IRepository<Account> accountRepository)
        {
            _tokenService = tokenService;
            _accountRepository = accountRepository;
        }

        [HttpPost("log-in")] // POST : api/account/login
        public ActionResult Login(LoginDTO loginDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginDTO.UserName) || string.IsNullOrEmpty(loginDTO.Password))
                {
                    return BadRequest(new { message = "Username and password are required" });
                }

                var userAccount = _accountRepository.GetByFilter(x => x.UserName == loginDTO.UserName && x.Password == loginDTO.Password).Include(x => x.User).Single();

                if (userAccount == null)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                var token = _tokenService.GenerateToken(userAccount);

                return Ok(new { token = token, role = userAccount.Role, user = userAccount.User });
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { message = "An error occurred while processing your request" });
            }
        }

        [HttpGet("log-out")]
        [Authorize]
        public ActionResult Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok();
        }
    }
}
