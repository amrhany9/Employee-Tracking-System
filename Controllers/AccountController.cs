using AutoMapper;
using back_end.DTOs;
using back_end.Models;
using back_end.Repositories;
using back_end.Services.Token;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ITokenService _tokenService;
        private IRepository<Account> _accountRepository;
        private IRepository<Employee> _employeeRepository;
        private IMapper _mapper;

        public AccountController(ITokenService tokenService, IRepository<Account> accountRepository, IRepository<Employee> employeeRepository, IMapper mapper)
        {
            _tokenService = tokenService;
            _accountRepository = accountRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [HttpPost("Login")]
        public ActionResult Login(LoginDTO loginDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginDTO.username) || string.IsNullOrEmpty(loginDTO.password))
                {
                    return BadRequest(new { message = "Username and password are required" });
                }

                var account = _accountRepository.GetByFilter(x => x.Username == loginDTO.username && x.Password == loginDTO.password)
                    .Include(x => x.Role)
                    .FirstOrDefault();

                if (account == null)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                var accessToken = _tokenService.GenerateJwtToken(account);
                var refreshToken = _tokenService.GenerateRefreshToken();

                account.RefreshToken = refreshToken;
                account.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

                _accountRepository.SaveChanges();

                return Ok(new { accessToken = accessToken, refreshToken = refreshToken});
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { message = $"An error occurred while processing your request: {ex}" });
            }
        }

        [HttpPost("Refresh")]
        public ActionResult RefreshToken(string refreshToken)
        {
            var account = _accountRepository.GetByFilter(a => a.RefreshToken == refreshToken).SingleOrDefault();
            if (account == null || account.RefreshTokenExpiry < DateTime.UtcNow)
                return Unauthorized("Invalid or expired refresh token");

            var newAccessToken = _tokenService.GenerateJwtToken(account);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            account.RefreshToken = newRefreshToken;
            account.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _accountRepository.SaveChanges();

            return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
        }


        [HttpPost("Logout")]
        public ActionResult Logout()
        {
            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(employeeId))
            {
                return Unauthorized("User not found");
            }

            var account = _accountRepository
                .GetByFilter(a => a.EmployeeId == int.Parse(employeeId))
                .SingleOrDefault();

            if (account == null) return NotFound();

            account.RefreshToken = null;
            account.RefreshTokenExpiry = null;
            _accountRepository.SaveChanges();

            return Ok("Logged out successfully");
        }


        [HttpPost("Register")]
        public ActionResult RegisterAccount(CreateAccountDTO accountDTO)
        {
            if (accountDTO == null)
            {
                return BadRequest("Invalid Account Data Provided.");
            }

            if (!_employeeRepository.GetByFilter(x => x.Id == accountDTO.userId).Any())
            {
                return BadRequest("Employee Data Is Not Found.");
            }

            if (_accountRepository.GetByFilter(x => x.EmployeeId == accountDTO.userId).Any())
            {
                return BadRequest("Employee Already Has An Account.");
            }

            _accountRepository.Add(_mapper.Map<Account>(accountDTO));
            _accountRepository.SaveChanges();

            return Ok(new { message = "Account Created successfully." });
        }
    }
}
