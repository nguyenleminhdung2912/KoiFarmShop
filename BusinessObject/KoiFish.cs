using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class KoiFish
{
    public long KoiFishId { get; set; }

    public string? Name { get; set; }

    public string? Origin { get; set; }

    public string? Gender { get; set; }

    public int? Age { get; set; }

    public int? Size { get; set; }

    public string? Breed { get; set; }

    public double? FilterRatio { get; set; }

    public byte[]? ImageData { get; set; }

    public double? Price { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<KoiFishRating> KoiFishRatings { get; set; } = new List<KoiFishRating>();
}
