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

        [HttpGet("users/department/{departmentId}")]
        public async Task<IActionResult> GetUsersByDepartment(int departmentId, int pageNumber = 1, int pageSize = 10)
        {
            var usersQuery = _userRepository.GetByFilter(u => u.DepartmentId == departmentId).AsQueryable();

            var usersPaged = await usersQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalUsers = await usersQuery.CountAsync(); 

            var result = new
            {
                users = usersPaged,
                totalCount = totalUsers
            };

            return Ok(result);
        }

    }
}
