using Common.Models;

namespace DataAccess.Services;

public class OrderRepository
{
    private readonly BookStoreDbContext _context;

    public OrderRepository()
    {
        _context = new BookStoreDbContext();
    }

    public OrderRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public void AddOrder(OrderModel order)
    {
        _context.Add
        (
            new OrderModel()
            {
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Customer = order.Customer
            }
        );
        _context.SaveChanges();
    }

    public List<OrderModel> GetAll()
    {
        return _context.Orders.Select
        (
            order => new OrderModel()
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
            }
        ).ToList();
    }
}