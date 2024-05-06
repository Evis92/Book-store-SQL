using Common.Models;
using DataAccess.Entities;

namespace DataAccess.Services;

public class CustomerRepository
{
    private readonly BookStoreDbContext _context;

    public  CustomerRepository()
    {
        _context = new BookStoreDbContext();
    }

    public CustomerRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public void AddCustomer(CustomerModel customer)
    {
        _context.Add
        (
            new Customer()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                City = customer.City,
                PostalCode = customer.PostalCode,
                Country = customer.Country,
                CustomerCategory = customer.CustomerCategory,
            }
        );
        _context.SaveChanges();
    }

    public List<CustomerModel> GetAll()
    {
        return _context.Customers.Select
        (
            customer => new CustomerModel()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                City = customer.City,
                PostalCode = customer.PostalCode,
                Country = customer.Country,
                CustomerCategory = customer.CustomerCategory,
            }
        ).ToList();
    }
}