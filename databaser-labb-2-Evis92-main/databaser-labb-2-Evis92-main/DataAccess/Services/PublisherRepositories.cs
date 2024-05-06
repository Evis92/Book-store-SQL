using Common.Models;
using DataAccess.Entities;

namespace DataAccess.Services;

public class PublisherRepository
{
    private readonly BookStoreDbContext _context;

    public PublisherRepository()
    {
        _context = new BookStoreDbContext();
    }

    public PublisherRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    private static PublisherModel _publisher;

    public static PublisherModel Publisher
    {
        get => _publisher;
        set
        {
            _publisher = value;
            PublisherChanged.Invoke();
        } 
    }

    public static event Action PublisherChanged;

    public void AddPublisher(PublisherModel publisher)
    {
        _context.Add
        (
            new Publisher()
            {
                Name = publisher.Name
            }
        );
        _context.SaveChanges();
        PublisherChanged.Invoke();
    }

    public List<PublisherModel> GetAllPublishers()
    {
        return _context.Publishers.Select
        (
            publisher => new PublisherModel()
            {
                PublisherId = publisher.PublisherId,
                Name = publisher.Name
            }
        ).ToList();
    }

    public bool PublisherExists(string publisherName)
    {
        return _context.Publishers.Any(p => p.Name == publisherName);
    }
}