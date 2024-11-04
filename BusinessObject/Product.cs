using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Product
{
    public long ProductId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public double? Price { get; set; }

    public int? Quantity { get; set; }
    
    public byte[]? ImageData { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<ProductRating> ProductRatings { get; set; } = new List<ProductRating>();
}
