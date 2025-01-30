using back_end.Models;
using back_end.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private IRepository<Role> _roleRepository;
        private IRepository<Account> _accountRepository;

        public RoleController(IRepository<Role> roleRepository, IRepository<Account> accountRepository)
        {
            _roleRepository = roleRepository;
            _accountRepository = accountRepository;
        }

        [HttpGet("All")]
        public ActionResult GetAllRoles()
        {
            var roles = _roleRepository.GetAll().ToList();
            return Ok(roles);
        }

        [HttpGet("{roleId}")]
        public ActionResult GetRoleById(int roleId)
        {
            var role = _roleRepository.GetByFilter(x => x.roleId == roleId).FirstOrDefault();
            return Ok(role);
        }

        [HttpGet("Account/{accountId}")]
        public ActionResult GetRoleByAccountId(int accountId)
        {
            var role = _accountRepository.GetByFilter(x => x.accountId == accountId)
                .Include(x => x.role)
                .Select(x => x.role)
                .FirstOrDefault();

            return Ok(role);
        }
    }
}
