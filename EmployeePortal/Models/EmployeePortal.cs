using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.Models
{
    public class Employees
    {
        [Key]
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

        public Departments Department { get; set; }
    }


    // File: Models/Department.cs
    public class Departments
    {
        [Key]
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }

        public Locations Location { get; set; } // Optional nav
        public ICollection<Employees> Employees { get; set; } // Optional reverse nav
    }
    public class EmployeeCreateViewModel
    {
        public Employees Employee { get; set; }
        [ValidateNever] // Tells ASP.NET Core to skip validation
        public List<SelectListItem> Department { get; set; }
    }
    public class EmployeeViewModel
    {
        [Key]
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime HireDate { get; set; }
        public string DepartmentName { get; set; }

        
    }
    // File: Models/Location.cs
    public class Locations
    {
       
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
    }

    

   
}
