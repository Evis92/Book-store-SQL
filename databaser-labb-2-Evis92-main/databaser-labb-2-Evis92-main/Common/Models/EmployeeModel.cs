

namespace Common.Models;

public class EmployeeModel
{
    public int EmploymentId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? StoreId { get; set; }

    public string? Telephone { get; set; }

    public virtual StoreModel? Store { get; set; }
}