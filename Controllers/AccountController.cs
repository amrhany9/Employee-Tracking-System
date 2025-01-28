using AutoMapper;
using back_end.DTOs;
using back_end.Models;
using back_end.Repositories;
using back_end.Services.Token;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost("Login")] // POST : api/account/login
        public ActionResult Login(LoginDTO loginDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginDTO.username) || string.IsNullOrEmpty(loginDTO.password))
                {
                    return BadRequest(new { message = "Username and password are required" });
                }

                var account = _accountRepository.GetByFilter(x => x.username == loginDTO.username && x.password == loginDTO.password)
                    .Include(x => x.role)
                    .FirstOrDefault();

                if (account == null)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                var token = _tokenService.GenerateToken(account);

                return Ok(new { token = token, roleId = account.roleId, roleName = account.role.roleNameEn});
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { message = $"An error occurred while processing your request: {ex}" });
            }
        }

        [HttpPost("Register-Account")]
        public ActionResult RegisterAccount(CreateAccountDTO accountDTO)
        {
            if (accountDTO == null)
            {
                return BadRequest("Invalid Account Data Provided.");
            }

            if (!_employeeRepository.GetByFilter(x => x.employeeId == accountDTO.userId).Any())
            {
                return BadRequest("Employee Data Is Not Found.");
            }

            if (_accountRepository.GetByFilter(x => x.employeeId == accountDTO.userId).Any())
            {
                return BadRequest("Employee Already Has An Account.");
            }

            _accountRepository.Add(_mapper.Map<Account>(accountDTO));
            _accountRepository.SaveChanges();

            return Ok(new { message = "Account Created successfully." });
        }
    }
}
