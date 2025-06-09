using System.Collections.Generic;
using System.Linq;
using EmployeePortal.Models.EmployeePortalEF;

namespace EmployeePortal.Services
{
    public static class EmployeeService
    {
        private static List<Employee> _employees = new List<Employee>
        {
            new Employee { EmployeeId = 1, Name = "Alice", Email = "alice@company.com" },
            new Employee { EmployeeId = 2, Name = "Bob",  Email = "bob@company.com" }
        };

        public static List<Employee> GetAll() => _employees;

        public static Employee GetById(int id) => _employees.FirstOrDefault(e => e.EmployeeId == id);

        public static void Add(Employee employee)
        {
            employee.EmployeeId = _employees.Max(e => e.EmployeeId) + 1;
            _employees.Add(employee);
        }
        
        public static void Update(Employee employee)
        {
            var index = _employees.FindIndex(e => e.EmployeeId == employee.EmployeeId);
            if (index >= 0)
                _employees[index] = employee;
        }

        public static void Delete(int id)
        {
            var emp = GetById(id);
            if (emp != null)
                _employees.Remove(emp);
        }
    }
}