using EmployeePortal.Models;
using EmployeePortal.Models.EmployeePortalEF;
using Department = EmployeePortal.Models.EmployeePortalEF.Department;

namespace EmployeePortal.Interfaces
{
    public interface IEmployeeEFService
    {
        Task<List<EmployeeEFViewModel>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
        Task<List<Department>> GetDepartmentsAsync();
        bool IsEmailDuplicate(string email, int? employeeId = null);
    }
}
