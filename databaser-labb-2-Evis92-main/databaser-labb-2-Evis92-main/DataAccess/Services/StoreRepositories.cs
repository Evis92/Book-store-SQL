using Common.Models;
using DataAccess.Entities;

namespace DataAccess.Services;

public class StoreRepository
{

    private readonly BookStoreDbContext _context;

    public StoreRepository()
    {
        _context = new BookStoreDbContext();
    }

    public StoreRepository(BookStoreDbContext context)
    {
        _context = context;
    }


    public void AddStore(StoreModel store)
    {
        _context.Add
        (
            new StoreModel()
            {
                StoreId = store.StoreId,
                Name = store.Name,
                Address = store.Address,
                City = store.City,
                PostalCode = store.PostalCode,
                Country = store.Country,
            }
        );
        _context.SaveChanges();
    }


    public List<StoreModel> GetAllStores()
    {
        return _context.Stores.Select(
            store => new StoreModel()
            {
                StoreId = store.StoreId,
                Name = store.Name,
                Address = store.Address,
                City = store.City,
                PostalCode = store.PostalCode,
                Country = store.Country,
                Inventories = store.Inventories.Select(
                    inventory => new InventoryModel
                    {
                        StoreId = inventory.StoreId,
                        Isbn = inventory.Isbn,
                        Quantity = inventory.Quantity
                    }).ToList(),
                Employees = store.Employees.Select(
                    employee => new EmployeeModel
                    {
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        StoreId = employee.StoreId,
                        Telephone = employee.Telephone,
                    }).ToList()

            }).ToList();
            
    }
}



