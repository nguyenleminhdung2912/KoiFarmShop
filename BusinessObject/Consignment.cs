using System;
using System.Collections;
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

    public byte[]? ImageData { get; set; }

    public string? Status { get; set; }
    //  PENDING là khi người dùng vừa đăng chờ duyệt
    //  REJECTED là khi staff từ chối yêu cầu này
    //  APPROVED là khi staff đồng ý yêu cầu, và cập nhật giá cho người dùng xem
    //  CONFIRMED là khi người dùng đồng ý
    //  CANCELLED là khi người dùng từ chối

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User? User { get; set; }
}
