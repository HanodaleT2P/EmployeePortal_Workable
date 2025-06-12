using EmployeePortal.Models.EmployeePortalEF;

namespace EmployeePortal.Interfaces
{
    public interface IEmployeeAPIConsume
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<EmployeeEFViewModel?> GetEmployeeByIdAsync(int id);
        Task<List<Department>> GetDepartmentsAsync();
        Task<EmployeeEFViewModel> AddEmployeeAsync(EmployeeAPICreateViewModel model);
        Task<EmployeeEFViewModel?> UpdateEmployeeAsync(int id, EmployeeAPICreateViewModel model);
    }
}
