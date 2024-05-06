namespace Common.Models;

public class AuthorModel
{
    public int AuthorId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public virtual ICollection<BookDetailModel> BookDetails { get; set; } = new List<BookDetailModel>();
}