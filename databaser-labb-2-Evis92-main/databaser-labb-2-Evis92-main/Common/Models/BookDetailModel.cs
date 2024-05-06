
namespace Common.Models;

public class BookDetailModel
{
    public string Isbn { get; set; } = null!;

    public int AuthorId { get; set; }

    public int PublisherId { get; set; }

    public virtual AuthorModel Author { get; set; } = null!;

    public virtual BookModel IsbnNavigation { get; set; } = null!;

    public virtual PublisherModel Publisher { get; set; } = null!;
}