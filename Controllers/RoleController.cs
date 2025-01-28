using back_end.Models;
using back_end.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRepository<Role> _roleRepository;

        public RoleController(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet("Get-All")]
        public ActionResult GetAllRoles()
        {
            var roles = _roleRepository.GetAll().ToList();
            return Ok(roles);
        }

        [HttpPost("Get-By-Id")]
        public ActionResult GetRoleById(int roleId)
        {
            var role = _roleRepository.GetByFilter(x => x.roleId == roleId).FirstOrDefault();
            return Ok(role);
        }
    }
}
