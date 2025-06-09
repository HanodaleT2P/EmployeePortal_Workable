using EmployeePortal.Interfaces;
using EmployeePortal.Models;
using EmployeePortal.Models.EmployeePortalEF;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace EmployeePortal.Services
{
    public class EmployeeEFService : IEmployeeEFService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeEFService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeEFViewModel>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Select(e => new EmployeeEFViewModel
                {
                    EmployeeId = e.EmployeeId,
                    Name = e.Name,
                    Email = e.Email,
                    HireDate = e.HireDate,
                    DepartmentName = e.Department.Name
                })
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp != null)
            {
                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
            }
        }
        public bool IsEmailDuplicate(string email, int? employeeId = null)
        {
            if (employeeId.HasValue)
            {
                return _context.Employees
                    .Any(e => e.Email == email && e.EmployeeId != employeeId.Value);
            }
            else
            {
                return _context.Employees
                    .Any(e => e.Email == email);
            }
        }
        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }
    }
   
}
