using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject;

public partial class User
{
    public long UserId { get; set; }

    public string? Name { get; set; }

    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email must be a Gmail address.")]
    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public string? Role { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Consignment> Consignments { get; set; } = new List<Consignment>();

    public virtual ICollection<KoiFishRating> KoiFishRatings { get; set; } = new List<KoiFishRating>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductRating> ProductRatings { get; set; } = new List<ProductRating>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
