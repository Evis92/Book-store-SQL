using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class BookDetail
{
    public string Isbn { get; set; } = null!;

    public int AuthorId { get; set; }

    public int PublisherId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Book IsbnNavigation { get; set; } = null!;

    public virtual Publisher Publisher { get; set; } = null!;
}
