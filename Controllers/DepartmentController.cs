using back_end.Models;
using back_end.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private IRepository<Department> _departmentRepository;
        private IRepository<Employee> _employeeRepository;

        public DepartmentController(IRepository<Department> departmentRepository, IRepository<Employee> employeeRepository)
        {
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpGet("All")]
        public ActionResult GetAllDepartments()
        {
            return Ok(_departmentRepository.GetAll().ToList());
        }

        [HttpGet("Employees/{departmentId}")]
        public async Task<IActionResult> GetEmployeesByDepartmentId(int departmentId, int pageNumber = 1, int pageSize = 10)
        {
            var employeesQuery = _employeeRepository.GetByFilter(u => u.DepartmentId == departmentId).AsQueryable();

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
