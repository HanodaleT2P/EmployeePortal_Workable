using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.Models
{
    public class Employees
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hire Date is required")]
        public DateTime HireDate { get; set; }


        [Required(ErrorMessage = "Department is required")]
        public int? DepartmentId { get; set; }  // Nullable int
    }


    // File: Models/Department.cs
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
    }
    public class EmployeeCreateViewModel
    {
        public Employees Employee { get; set; }
        [ValidateNever] // Tells ASP.NET Core to skip validation
        public List<SelectListItem> Department { get; set; }
    }
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime HireDate { get; set; }
        public string DepartmentName { get; set; }
    }
    // File: Models/Location.cs
    public class Location
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
    }

    // File: Models/Project.cs
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    // File: Models/ProjectAssignment.cs
    public class ProjectAssignment
    {
        public int AssignmentId { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public DateTime AssignmentDate { get; set; }
    }
}
