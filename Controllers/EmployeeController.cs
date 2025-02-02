using back_end.Models;
using back_end.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IRepository<Employee> _employeeRepository;

        public EmployeeController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("All")]
        public ActionResult<IEnumerable<Employee>> GetEmployees()
        {
            return Ok(_employeeRepository.GetAll().ToList());
        }

        [HttpGet]
        public ActionResult<Employee> GetEmployeeByToken()
        {
            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var employee = _employeeRepository.GetByFilter(x => x.employeeId == int.Parse(employeeId)).FirstOrDefault();

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpGet("{employeeId}")]
        public ActionResult<Employee> GetEmployeeById(int employeeId)
        {
            var employee = _employeeRepository.GetByFilter(x => x.employeeId == employeeId).FirstOrDefault();

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            return Ok(employee);
        }
    }
}
