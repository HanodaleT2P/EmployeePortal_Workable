// EmployeeRepository.cs
using Microsoft.EntityFrameworkCore;
using EmployeePortal.Models;
using EmployeePortal.Models.EmployeePortalEF;
using EmployeePortal.Interfaces;

public class EmployeeAPIConsume : IEmployeeAPIConsume
{
    private readonly ApplicationDbContext _context;

    public EmployeeAPIConsume(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        return await _context.Employees.Include(e => e.Department).ToListAsync();
    }

    public async Task<EmployeeEFViewModel?> GetEmployeeByIdAsync(int id)
    {
        return await _context.Employees
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
    }

    public async Task<List<Department>> GetDepartmentsAsync()
    {
        return await _context.Departments.ToListAsync();
    }

    public async Task<EmployeeEFViewModel> AddEmployeeAsync(EmployeeAPICreateViewModel model)
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

        var department = await _context.Departments.FindAsync(employee.DepartmentId);

        return new EmployeeEFViewModel
        {
            EmployeeId = employee.EmployeeId,
            Name = employee.Name,
            Email = employee.Email,
            HireDate = employee.HireDate,
            DepartmentId = employee.DepartmentId,
            DepartmentName = department?.Name ?? "N/A"
        };
    }

    public async Task<EmployeeEFViewModel?> UpdateEmployeeAsync(int id, EmployeeAPICreateViewModel model)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
            return null;

        employee.Name = model.Name;
        employee.Email = model.Email;
        employee.HireDate = model.HireDate;
        employee.DepartmentId = model.DepartmentId;

        await _context.SaveChangesAsync();

        var department = await _context.Departments.FindAsync(employee.DepartmentId);

        return new EmployeeEFViewModel
        {
            EmployeeId = employee.EmployeeId,
            Name = employee.Name,
            Email = employee.Email,
            HireDate = employee.HireDate,
            DepartmentId = employee.DepartmentId,
            DepartmentName = department?.Name ?? "N/A"
        };
    }
}
