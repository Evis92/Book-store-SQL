using Common.Models;

namespace DataAccess.Services;

public class OrderDetailRepository
{
    private readonly BookStoreDbContext _context;

    public OrderDetailRepository()
    {
        _context = new BookStoreDbContext();
    }

    public OrderDetailRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public void AddOrderDetail(OrderDetailModel orderDetail)
    {
        _context.Add
        (
            new OrderDetailModel()
            {
                Isbn = orderDetail.Isbn,
                OrderId = orderDetail.OrderId,
                Quantity = orderDetail.Quantity
            }
        );
        _context.SaveChanges();
    }

    public List<OrderDetailModel> GetAll()
    {
        return _context.OrderDetails.Select
        (
            orderDetail => new OrderDetailModel()
            {
                Isbn = orderDetail.Isbn,
                OrderId = orderDetail.OrderId,
                Quantity = orderDetail.Quantity
            }
        ).ToList();
    }
}