using back_end.Data;
using back_end.Models;
using back_end.Repositories;
using back_end.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet] // GET : api/users
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _userRepository.GetAll();

            return Ok(users);
        }

        [HttpGet("{id}")] // GET : api/users/1
        public ActionResult<User> GetUser(int id)
        {
            var user = _userRepository.GetById(id);

            return Ok(user);
        }
    }
}
