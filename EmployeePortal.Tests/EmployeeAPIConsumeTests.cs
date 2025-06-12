using Xunit;
using Microsoft.EntityFrameworkCore;
using EmployeePortal.Controllers;
using EmployeePortal.Models;
using EmployeePortal.Models.EmployeePortalEF;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
public class EmployeeAPIConsumeTests
{
    public ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "EmployeePortal")
            .Options;

        var context = new ApplicationDbContext(options);

        if (!context.Departments.Any())
        {
            context.Departments.AddRange(
                new Department { DepartmentId = 1, Name = "HR" },
                new Department { DepartmentId = 2, Name = "IT" }
            );
            context.Employees.Add(new Employee
            {
                EmployeeId = 1,
                Name = "Alice",
                Email = "alice@example.com",
                HireDate = DateTime.Now,
                DepartmentId = 1
            });
            context.SaveChanges();
        }

        return context;
    }

    [Fact]
    public async Task GetEmployeeList_ReturnsEmployees()
    {
        var context = GetDbContext();
        var controller = new EmployeeAPIController(context);

        var result = await controller.GetEmployeeList() as OkObjectResult;

        Assert.NotNull(result);
        var employees = Assert.IsType<List<Employee>>(result.Value);
        Assert.True(employees.Count > 0);
    }

    [Fact]
    public async Task GetDepartmentList_ReturnsDepartments()
    {
        var context = GetDbContext();
        var controller = new EmployeeAPIController(context);

        var result = await controller.GetDepartments() as OkObjectResult;

        Assert.NotNull(result);
        var departments = Assert.IsType<List<Department>>(result.Value);
        Assert.Equal(2, departments.Count);
    }

    [Fact]
    public async Task GetEmployeeById_ReturnsCorrectEmployee()
    {
        var context = GetDbContext();
        var controller = new EmployeeAPIController(context);

        var result = await controller.GetEmployeeById(1) as OkObjectResult;

        Assert.NotNull(result);
        var employee = Assert.IsType<EmployeeEFViewModel>(result.Value);
        Assert.Equal("Alice", employee.Name);
    }

    [Fact]
    public async Task AddEmployee_AddsNewEmployee()
    {
        var context = GetDbContext();
        var controller = new EmployeeAPIController(context);

        var newEmp = new EmployeeAPICreateViewModel
        {
            Name = "Bob",
            Email = "bob@example.com",
            HireDate = DateTime.Now,
            DepartmentId = 2
        };

        var result = await controller.AddEmployee(newEmp) as OkObjectResult;

        Assert.NotNull(result);
        var emp = Assert.IsType<Employee>(result.Value);
        Assert.Equal("Bob", emp.Name);
        Assert.Equal(2, emp.DepartmentId);
    }

    [Fact]
    public async Task EditEmployee_UpdatesExistingEmployee()
    {
        var context = GetDbContext();
        var controller = new EmployeeAPIController(context);

        var updatedEmp = new EmployeeAPICreateViewModel
        {
            Name = "Alice Updated",
            Email = "alice.new@example.com",
            HireDate = DateTime.Now,
            DepartmentId = 2
        };

        var result = await controller.EditEmployee(1, updatedEmp) as OkObjectResult;

        Assert.NotNull(result);
        var emp = Assert.IsType<Employee>(result.Value);
        Assert.Equal("Alice Updated", emp.Name);
        Assert.Equal(2, emp.DepartmentId);
    }
}
