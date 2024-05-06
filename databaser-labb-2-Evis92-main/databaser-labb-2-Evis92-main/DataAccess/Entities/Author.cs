using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Author
{
    public int AuthorId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public virtual ICollection<BookDetail> BookDetails { get; set; } = new List<BookDetail>();
}
