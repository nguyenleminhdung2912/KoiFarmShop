using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class KoiFishRating
{
    public long KoiFishRatingId { get; set; }

    public long? KoiFishId { get; set; }

    public long? UserId { get; set; }

    public int? Rating { get; set; }

    public string? Description { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual KoiFish? KoiFish { get; set; }

    public virtual User? User { get; set; }
}
