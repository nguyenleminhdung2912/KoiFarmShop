using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class ProductRating
{
    public long ProductRatingId { get; set; }

    public long? ProductId { get; set; }

    public long? UserId { get; set; }

    public int? Rating { get; set; }

    public string? Description { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
