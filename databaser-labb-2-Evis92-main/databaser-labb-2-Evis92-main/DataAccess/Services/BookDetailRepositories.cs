using Common.Models;
using DataAccess.Entities;

namespace DataAccess.Services;

public class BookDetailRepository
{
    private readonly BookStoreDbContext _context;
    public BookDetailRepository()
    {
        _context = new BookStoreDbContext();
    }

    public BookDetailRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public void AddBookDetail(BookDetailModel bookDetail)
    {
        _context.Add
        (
            new BookDetail()
            {
                Isbn = bookDetail.Isbn ,
                AuthorId= bookDetail.AuthorId,
                PublisherId = bookDetail.PublisherId
            }
        );
        _context.SaveChanges();
    }


    public List<BookDetailModel> GetAll()
    {
        return _context.BookDetails.Select
        (
            bookDetail => new BookDetailModel()
            {
                Isbn = bookDetail.Isbn,
                AuthorId= bookDetail.AuthorId,
                PublisherId= bookDetail.PublisherId,
                
            }
        ).ToList();
    }
}