﻿using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class VTitlesByAuthor
{
    public string Name { get; set; } = null!;

    public int? Age { get; set; }

    public int? Titles { get; set; }

    public decimal? InventoryValue { get; set; }
}
