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

        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetEmployees()
        {
            return Ok(_employeeRepository.GetAll().ToList());
        }

        [HttpGet("{employeeId}")]
        public ActionResult<Employee> GetEmployeeById(int employeeId)
        {
            var employee = _employeeRepository.GetByFilter(x => x.employeeId == employeeId).FirstOrDefault();

            return Ok(employee);
        }

        [HttpGet("me")]
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

        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> GetEmployeesByDepartment(int departmentId, int pageNumber = 1, int pageSize = 10)
        {
            var employeesQuery = _employeeRepository.GetByFilter(u => u.departmentId == departmentId).AsQueryable();

            var employeesPaged = await employeesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalemployees = await employeesQuery.CountAsync(); 

            var result = new
            {
                employees = employeesPaged,
                totalCount = totalemployees
            };

            return Ok(result);
        }

    }
}
