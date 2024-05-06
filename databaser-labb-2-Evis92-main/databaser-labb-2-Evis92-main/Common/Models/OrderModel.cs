

namespace Common.Models;

public class OrderModel
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateOnly OrderDate { get; set; }

    public virtual CustomerModel Customer { get; set; } = null!;

    public virtual ICollection<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();
}