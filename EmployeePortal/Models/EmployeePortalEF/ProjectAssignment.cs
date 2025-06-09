using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.Models.EmployeePortalEF;
[Keyless]
public partial class ProjectAssignment
{
 
    public int AssignmentId { get; set; }

    public DateTime AssignmentDate { get; set; }

    public int ProjectId { get; set; }

    public int EmployeeId { get; set; }

    public Employee Employee { get; set; }

    public Project Project { get; set; }
}
