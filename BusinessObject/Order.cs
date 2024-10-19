using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public partial class Order
{
    public long OrderId { get; set; }

    public long? UserId { get; set; }

    public string? KoiFishId { get; set; }

    public string? ProductId { get; set; }

    public int? Quantity { get; set; }

    public double? TotalPrice { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User? User { get; set; }

    [NotMapped]
    public List<KoiFish> KoiFishList { get; set; } = new List<KoiFish>();

    [NotMapped]
    public List<Product> ProductList { get; set; } = new List<Product>();
}
