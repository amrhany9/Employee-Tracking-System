using back_end.Data;
using back_end.Models;
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
        private UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet] // GET : api/users
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _userService.GetAllUsers();

            return Ok(users);
        }

        [HttpGet("{id}")] // GET : api/users/1
        public ActionResult<User> GetUser(int id)
        {
            var user = _userService.GetUser(id);

            return Ok(user);
        }
    }
}
