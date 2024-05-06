using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Employee
{
    public int EmploymentId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? StoreId { get; set; }

    public string? Telephone { get; set; }

    public virtual Store? Store { get; set; }
}
