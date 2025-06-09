using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.Models.EmployeePortalEF;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public int DepartmentId { get; set; }
    [ValidateNever]

    public Department Department { get; set; } 

    public ICollection<ProjectAssignment> ProjectAssignments { get; set; }
           = new List<ProjectAssignment>();  // Initialize collection
}

public class EmployeeEFCreateViewModel
{
    public Employee Employee { get; set; }
    [ValidateNever] // Tells ASP.NET Core to skip validation
    public List<SelectListItem> Department { get; set; }
}
public class EmployeeEFViewModel
{
    [Key]
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime HireDate { get; set; }
    public string DepartmentName { get; set; }


}