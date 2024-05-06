

namespace Common.Models;

public class PublisherModel
{
    public int PublisherId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<BookDetailModel> BookDetails { get; set; } = new List<BookDetailModel>();
}