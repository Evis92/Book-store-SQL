using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public int? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? CustomerCategory { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
