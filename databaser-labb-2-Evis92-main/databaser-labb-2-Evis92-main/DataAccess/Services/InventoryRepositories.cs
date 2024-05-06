using Common.Models;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services;

public class InventoryRepository
{
    private readonly BookStoreDbContext _context;

    public InventoryRepository()
    {
        _context = new BookStoreDbContext();
    }

    public InventoryRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public static event Action InventoryChanged;

    public event Action<string> BookRemovedFromInventory;


    protected virtual void OnInventoryChanged()
    {
        InventoryChanged?.Invoke();
    }


    public void AddInventory(InventoryModel inventory)
    {
        _context.Add
        (
            new InventoryModel()
            {
                StoreId = inventory.StoreId,
                Isbn = inventory.Isbn,
                Quantity = inventory.Quantity
            }
        );
        _context.SaveChanges();
    }


    public List<InventoryModel> GetInventoryForStore(int storeId)
    {
        return _context.Inventories
            .Include(i => i.IsbnNavigation)
            .Include(i => i.Store)
            .Where(i => i.StoreId == storeId)
            .Select(inventory => new InventoryModel
            {
                StoreId = inventory.StoreId,
                Isbn = inventory.Isbn,
                Quantity = inventory.Quantity,
                IsbnNavigation = new BookModel
                {
                    Isbn13 = inventory.IsbnNavigation.Isbn13,
                    Title = inventory.IsbnNavigation.Title
                    // Fyll i andra egenskaper som du behöver
                },
                Store = new StoreModel
                {
                    StoreId = inventory.Store.StoreId,
                    Name = inventory.Store.Name
                    // Fyll i andra egenskaper som du behöver
                }
            })
            .ToList();
    }






















    public void AddBookToInventory(int storeId, BookModel book, int quantity)
    {
        // Kontrollera om det redan finns en post för den här boken i lagret
        var existingInventory = _context.Inventories
            .FirstOrDefault(i => i.StoreId == storeId && i.Isbn == book.Isbn13);

        if (existingInventory != null)
        {
            
            // Om boken redan finns i lagret, uppdatera antalet
            existingInventory.Quantity += quantity;

        }
        else
        {
            _context.Add(
                new Inventory()
                {
                    StoreId = storeId,
                    Isbn = book.Isbn13,
                    Quantity = quantity,
                }
            );
        }

        _context.SaveChanges();

        OnInventoryChanged();
    }


    public void RemoveBookFromInventory(int storeId, BookModel book, int quantity)
    {
        var inventoryBook = _context.Inventories
            .FirstOrDefault(i => i.StoreId == storeId && i.Isbn == book.Isbn13);
        if (inventoryBook != null)
        {
            if (inventoryBook.Quantity > 1)
            {
                inventoryBook.Quantity -= quantity;
            }
            else
            {
                _context.Inventories.Remove( inventoryBook );
            }

            _context.SaveChanges();
            OnInventoryChanged();
        }
        else
        {
            BookRemovedFromInventory.Invoke("The book does not exist in Inventory");
        }

    }

}