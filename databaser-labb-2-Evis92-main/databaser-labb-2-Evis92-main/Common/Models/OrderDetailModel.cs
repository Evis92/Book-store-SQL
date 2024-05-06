
namespace Common.Models;

public class OrderDetailModel
{
    public int OrderId { get; set; }

    public string Isbn { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual BookModel IsbnNavigation { get; set; } = null!;

    public virtual OrderModel Order { get; set; } = null!;
}