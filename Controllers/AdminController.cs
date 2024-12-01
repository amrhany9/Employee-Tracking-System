using AutoMapper;
using back_end.DTOs;
using back_end.Mediators.Attendance;
using back_end.Models;
using back_end.Repositories;
using back_end.Services.Attendance;
using back_end.Services.Location;
using back_end.Services.ZKEM_Machine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private ILocationService _locationService;
        private IMachineService _machineService;
        private IAttendanceService _attendanceService;
        private IRepository<User> _userRepository;
        private IRepository<Account> _accountRepository;
        private IMapper _mapper;

        public AdminController(ILocationService locationService, IMachineService machineService, IAttendanceService attendanceService, IRepository<User> userRepository, IRepository<Account> accountRepository, IMapper mapper)
        {
            _locationService = locationService;
            _machineService = machineService;
            _attendanceService = attendanceService;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        [HttpGet("Get-Users-Data")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(_userRepository.GetAll());
        }

        [HttpPost("Register-Account")]
        public ActionResult RegisterAccount(CreateAccountDTO accountDTO)
        {
            if (accountDTO == null)
            {
                return BadRequest("Invalid Account Data Provided.");
            }

            if (!_userRepository.GetById(accountDTO.UserId).Any())
            {
                return BadRequest("User Data Is Not Found.");
            }

            if (_accountRepository.GetByFilter(x => x.UserId == accountDTO.UserId).Any())
            {
                return BadRequest("User Already Has An Account.");
            }

            _accountRepository.Add(_mapper.Map<Account>(accountDTO));
            _accountRepository.SaveChanges();

            return Ok(new { message = "Account Created successfully."});
        }

        [HttpGet("Get-Daily-Log")]
        public ActionResult<IEnumerable<Attendance>> GetDailyAttendanceLog()
        {
            return Ok(_attendanceService.GetDailyLog());
        }

        [HttpPost("Connect-Machine")]
        public ActionResult ConnectToZkemDevice(string Ip, int Port)
        {
            _machineService.setDeviceNetwork(Ip, Port);
            if (_machineService.isConnected())
            {
                return Ok("Device Connected Successfully");
            }
            else
            {
                return BadRequest($"Failed to connect to the device : {_machineService.GetLastError()}");
            }
        }

        [HttpPost("Set-Company-Location")]
        public ActionResult SetCompanyLocation(double Latitude, double Longitude, double Radius)
        {
            _locationService.SetCompanyCoordinates(Latitude, Longitude);
            _locationService.SetAllowedRadius(Radius);

            return Ok(new { message = "Data set successfully." });
        }
    }
}
