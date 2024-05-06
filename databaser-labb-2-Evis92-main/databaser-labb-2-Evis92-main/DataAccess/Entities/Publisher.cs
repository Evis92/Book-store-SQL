using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<BookDetail> BookDetails { get; set; } = new List<BookDetail>();
}
