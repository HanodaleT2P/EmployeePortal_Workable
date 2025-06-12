using EmployeePortal.Models.EmployeePortalEF;
using EmployeePortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeAPI
        [HttpGet("GetEmployeeList")]
        public async Task<IActionResult> GetEmployeeList()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .ToListAsync();
            return Ok(employees);
        }

        // GET: api/EmployeeAPI/departments
        [HttpGet("GetDepartmentList")]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _context.Departments.ToListAsync();
            return Ok(departments);
        }

        // GET: api/EmployeeAPI/5
        [HttpGet("GetEmployeeById/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.EmployeeId == id)
                .Select(e => new EmployeeEFViewModel
                {
                    EmployeeId = e.EmployeeId,
                    Name = e.Name,
                    Email = e.Email,
                    HireDate = e.HireDate,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department.Name
                })
                .FirstOrDefaultAsync();

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // POST: api/EmployeeAPI
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee(EmployeeAPICreateViewModel model)
        {
            var employee = new Employee
            {
                Name = model.Name,
                Email = model.Email,
                HireDate = model.HireDate,
                DepartmentId = model.DepartmentId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        // PUT: api/EmployeeAPI/5
        [HttpPut("EditEmployee/{id}")]
        public async Task<IActionResult> EditEmployee(int id, EmployeeAPICreateViewModel model)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            employee.Name = model.Name;
            employee.Email = model.Email;
            employee.HireDate = model.HireDate;
            employee.DepartmentId = model.DepartmentId;

            await _context.SaveChangesAsync();

            

            return Ok(employee);
        }
    }
}
