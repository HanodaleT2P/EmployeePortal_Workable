using System.Collections.Generic;
using System.Linq;
using EmployeePortal.Models;

namespace EmployeePortal.Services
{
    public static class EmployeeService
    {
        private static List<Employee> _employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "Alice", Department = "HR", Email = "alice@company.com" },
            new Employee { Id = 2, Name = "Bob", Department = "IT", Email = "bob@company.com" }
        };

        public static List<Employee> GetAll() => _employees;

        public static Employee GetById(int id) => _employees.FirstOrDefault(e => e.Id == id);

        public static void Add(Employee employee)
        {
            employee.Id = _employees.Max(e => e.Id) + 1;
            _employees.Add(employee);
        }

        public static void Update(Employee employee)
        {
            var index = _employees.FindIndex(e => e.Id == employee.Id);
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