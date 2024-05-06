using Common.Models;
using DataAccess.Entities;

namespace DataAccess.Services;

public class EmployeeRepository
{
    private readonly BookStoreDbContext _context;

    public EmployeeRepository()
    {
        _context = new BookStoreDbContext();
    }

    public EmployeeRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public void AddEmployee(EmployeeModel employee)
    {
        _context.Add
        (
            new Employee()
            {
                //EmploymentId = employee.EmploymentId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                StoreId = employee.StoreId,
                Telephone = employee.Telephone,
            }
        );
        _context.SaveChanges();
    }

    public List<EmployeeModel> GetAll()
    {
        return _context.Employees.Select
        (
            employee => new EmployeeModel()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                StoreId = employee.StoreId,
                Telephone = employee.Telephone,
            }
        ).ToList();
    }

}
