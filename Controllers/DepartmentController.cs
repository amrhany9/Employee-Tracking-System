using back_end.Models;
using back_end.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private IRepository<Department> _departmentRepository;

        public DepartmentController(IRepository<Department> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet("Get-All")]
        public ActionResult<IEnumerable<Department>> GetAllDepartments()
        {
            return Ok(_departmentRepository.GetAll().ToList());
        }
    }
}
