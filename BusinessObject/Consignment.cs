using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Consignment
{
    public long ConsignmentId { get; set; }

    public long? UserId { get; set; }

    public string? KoiName { get; set; }

    public double? Price { get; set; }

    public DateTime? FromTime { get; set; }

    public DateTime? ToTime { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsDeleted { get; set; }


    public virtual User? User { get; set; }
}
