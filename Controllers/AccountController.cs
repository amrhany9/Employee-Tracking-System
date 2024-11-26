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
            var userAccount = _accountRepository.GetByFilter(x => x.UserName == loginDTO.UserName).SingleOrDefault();

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
        public ActionResult Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID could not be found.");
            }

            var userAccount = _accountRepository.GetById(int.Parse(userId)).SingleOrDefault();

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
    }
}
