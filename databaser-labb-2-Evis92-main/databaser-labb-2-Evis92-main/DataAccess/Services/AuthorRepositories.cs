using Common.Models;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services;

public class AuthorRepository
{
    private readonly BookStoreDbContext _context;

    public AuthorRepository()
    {
        _context = new BookStoreDbContext();
    }

    public AuthorRepository(BookStoreDbContext context)
    {
        _context = context;
    }


    private static AuthorModel _author;


    public static AuthorModel Author
    {
        get => _author;
        set
        {
            _author = value;
            AuthorChanged.Invoke();
        }
    }

    public static event Action AuthorChanged;

    public void AddAuthor(AuthorModel author)
    {
        _context.Add
        (
            new Author()
            {
                FirstName = author.FirstName,
                LastName = author.LastName,
                DateOfBirth = author.DateOfBirth,
            }
        );
        _context.SaveChanges();
        AuthorChanged.Invoke();
    }

    public List<AuthorModel> GetAllAuthors()
    {
        return _context.Authors.Select
        (
            author => new AuthorModel()
            {
                AuthorId = author.AuthorId,
                FirstName = author.FirstName,
                LastName = author.LastName,
                DateOfBirth = author.DateOfBirth,
            }
        ).ToList();
    }

    public AuthorModel GetById(int authorId)
    {
        var author = _context.Authors.Find(authorId);

        var newAuthor = new AuthorModel
        {
            AuthorId = author.AuthorId,
            FirstName = author.FirstName,
            LastName = author.LastName,
            DateOfBirth = author.DateOfBirth,

        };

        return newAuthor;
    }

    public bool AuthorExist(string firstName, string lastName, DateOnly dateOfBirth)
    {
        return _context.Authors.Any(a => 
            a.FirstName == firstName && 
            a.LastName == lastName &&
            a.DateOfBirth == dateOfBirth);
    }

}