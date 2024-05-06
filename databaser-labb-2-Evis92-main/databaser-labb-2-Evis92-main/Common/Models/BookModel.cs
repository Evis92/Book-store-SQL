
namespace Common.Models;

public class BookModel
{
    public string Isbn13 { get; set; } = null!;

    public string? Title { get; set; }

    public string? Language { get; set; }

    public string? Format { get; set; }

    public decimal? Price { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string? Translater { get; set; }

    public int? Pages { get; set; }

    public virtual ICollection<BookDetailModel> BookDetails { get; set; } = new List<BookDetailModel>();

    public virtual ICollection<InventoryModel> Inventories { get; set; } = new List<InventoryModel>();

    public virtual ICollection<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();
}