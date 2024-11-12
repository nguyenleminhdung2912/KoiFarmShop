using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Blog
{
    public long BlogId { get; set; }

    public long? UserId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

	public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User? User { get; set; }
}
