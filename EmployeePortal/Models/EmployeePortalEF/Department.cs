﻿using System;
using System.Collections.Generic;

namespace EmployeePortal.Models.EmployeePortalEF;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string Name { get; set; } = null!;

    public int LocationId { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
