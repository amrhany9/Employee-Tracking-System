using back_end.Data;
using back_end.Models;
using back_end.Repositories;
using back_end.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IRepository<User> _userRepository;

        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{id}")] // GET : api/users/1
        public ActionResult<User> GetUserById(int id)
        {
            var user = _userRepository.GetById(id).FirstOrDefault();

            return Ok(user);
        }

        [HttpGet("me")]
        public ActionResult<User> GetUserByToken()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = _userRepository.GetById(int.Parse(userId)).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("{departmentId}")]
        public ActionResult<IEnumerable<User>> GetUsersByDepartment(int departmentId)
        {
            var users = _userRepository.GetByFilter(x => x.DepartmentId == departmentId).ToList();

            return Ok(users);
        }
    }
}
