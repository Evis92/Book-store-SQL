
namespace Common.Models;

public class StoreModel
{
    public int StoreId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public int? PostalCode { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<EmployeeModel> Employees { get; set; } = new List<EmployeeModel>();

    public virtual ICollection<InventoryModel> Inventories { get; set; } = new List<InventoryModel>();
}