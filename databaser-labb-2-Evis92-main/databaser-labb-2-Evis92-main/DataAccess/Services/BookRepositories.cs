using Common.Models;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services;

public class BookRepository
{
    private readonly BookStoreDbContext _context;

    public BookRepository()
    {
        _context = new BookStoreDbContext();
    }

    public BookRepository(BookStoreDbContext context)
    {
        _context = context;
    }


    private static BookModel _book;


    public static BookModel Book
    {
        get => _book;
        set
        {
            _book = value;
            BookChanged.Invoke();
        }
    }

    public static event Action BookChanged;


    public void AddBook(BookModel book)
    {
        _context.Add
        (
            new Book()
            {
                Isbn13 = book.Isbn13,
                Title = book.Title,
                Language = book.Language,
                Format = book.Format,
                Price = book.Price,
                ReleaseDate = book.ReleaseDate,
                Translater = book.Translater,
                Pages = book.Pages,
                BookDetails = book.BookDetails.Select(
                    bookDetail => new BookDetail
                    {
                        Isbn = bookDetail.Isbn,
                        AuthorId = bookDetail.AuthorId,
                        PublisherId = bookDetail.PublisherId,
                    }).ToList(),
                Inventories = book.Inventories.Select(
                    inventory => new Inventory
                    {
                        StoreId = inventory.StoreId,
                        Isbn = inventory.Isbn,
                        Quantity = inventory.Quantity,
                    }).ToList(),
                OrderDetails = book.OrderDetails.Select(
                    orderDetail => new OrderDetail
                    {
                        OrderId = orderDetail.OrderId,
                        Isbn = orderDetail.Isbn,
                        Quantity = orderDetail.Quantity,
                    }).ToList(),
            });
        _context.SaveChanges();
        BookChanged.Invoke();
    }



    public bool BookExists(string isbn)
    {
        return _context.Books.Any(book => book.Isbn13 == isbn);
    }


    public List<BookModel> GetAllBooks()
    {
        return _context.Books.Select
        (
            book => new BookModel
            {
                Isbn13 = book.Isbn13,
                Title = book.Title,
                Language = book.Language,
                Format = book.Format,
                Price = book.Price,
                ReleaseDate = book.ReleaseDate,
                Translater = book.Translater,
                Pages = book.Pages,
            }
        ).ToList();
    }



    public BookModel GetByIsbn(string isbn13)
    {
        var book = _context.Books.Find(isbn13);

        var newbook = new BookModel
        {
            Isbn13 = book.Isbn13,
            Title = book.Title,
            Language = book.Language,
            Format = book.Format,
            Price = book.Price,
            ReleaseDate = book.ReleaseDate,
            Translater = book.Translater,
            Pages = book.Pages,
        };

        return newbook;
    }









    public void UpdateBook(BookModel book)
    {
        var existingBook = _context.Books
            .Include(b => b.BookDetails)
            .SingleOrDefault(b => b.Isbn13 == book.Isbn13);


        if (existingBook != null)
        {
            existingBook.Title = book.Title;
            existingBook.Language = book.Language;
            existingBook.Format = book.Format;
            existingBook.Price = book.Price;
            existingBook.ReleaseDate = book.ReleaseDate;
            existingBook.Translater = book.Translater;
            existingBook.Pages = book.Pages;


            foreach (var detail in existingBook.BookDetails.ToList())
            {
                _context.Remove(detail);
            }

   
            foreach (var bookDetail in book.BookDetails)
            {
                existingBook.BookDetails.Add(new BookDetail
                {
                    Isbn = existingBook.Isbn13,  
                    AuthorId = bookDetail.AuthorId,
                    PublisherId = bookDetail.PublisherId,
                });
            }

            _context.SaveChanges();
            BookChanged.Invoke();
        }
    }

    public void DeleteBook(string Isbn)
    {
        var book = _context.Books
            .Include(b => b.BookDetails)
            .Include(b => b.Inventories) 
            .FirstOrDefault(b => b.Isbn13 == Isbn);

        if (book != null)
        {

            var orderDetails = _context.OrderDetails.Where(od => od.Isbn == Isbn);
            _context.OrderDetails.RemoveRange(orderDetails);
            _context.BookDetails.RemoveRange(book.BookDetails);

            _context.Inventories.RemoveRange(book.Inventories);

            _context.Books.Remove(book);

            
            _context.SaveChanges();
            BookChanged.Invoke();
        }
    }




    public List<object> GetAllInfoOnBooks()
    {
        
            using (var dbContext = new BookStoreDbContext())
            {
                var authorBookDetails = dbContext.Books
                    .Join(
                        dbContext.BookDetails,
                        book => book.Isbn13,
                        bookDetails => bookDetails.Isbn,
                        (book, bookDetails) => new { book, bookDetails }
                    )
                    .Join(
                        dbContext.Authors,
                        combined => combined.bookDetails.AuthorId,
                        author => author.AuthorId,
                        (combined, author) => new { combined.book, combined.bookDetails, author }
                    )
                    .Join(
                        dbContext.Publishers,
                        combined => combined.bookDetails.PublisherId,
                        publisher => publisher.PublisherId,
                        (combined, publisher) => new
                        {
                            AuthorName = $"{combined.author.FirstName} {combined.author.LastName}",
                            BookTitle = combined.book.Title,
                            PublisherName = publisher.Name
                        }
                    ).ToList();

                return new List<object>(authorBookDetails);
            }
        
    }
}
