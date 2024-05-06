

namespace Common.Models;

public class InventoryModel
{
    public int StoreId { get; set; }

    public string Isbn { get; set; } = null!;

    public int? Quantity { get; set; }

    public virtual BookModel IsbnNavigation { get; set; } = null!;

    public virtual StoreModel Store { get; set; } = null!;
}