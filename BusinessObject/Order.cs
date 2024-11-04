using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public partial class Order
{
    public long OrderId { get; set; }

    public long? UserId { get; set; }

    public string? KoiFishId { get; set; }
    //1, 2, 4

    public string? ProductId { get; set; }

    public int? Quantity { get; set; }

    public double? TotalPrice { get; set; }

    public string? Status { get; set; }
    //  PAID là khi khách hàng đã thanh toán, ngồi chờ giao hàng
    //  COMPLETED là khi đơn hàng được giao thành công
    //  CANCELLED là khi người mua huỷ yêu cầu -> Hoàn tiền lại
    //                                         -> 1 ngày trước khi giao : chỉ hoàn 80%
    //                                         -> Còn lại               : hoàn 100%
    //  FAILED là khi đơn hàng bị fail do không đủ tiền trong tài khoản

    public string? ShipmentStatus { get; set; }
    // NOTYET là khi chưa làm gì hết
    // PREPARING là khi đang soạn hàng
    // ONGOING là khi đang giao
    // SUCCESSFUL là khi hoàn thành
    
    public string? Address { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User? User { get; set; }

    [NotMapped]
    public List<KoiFish> KoiFishList { get; set; } = new List<KoiFish>();

    [NotMapped]
    public List<Product> ProductList { get; set; } = new List<Product>();
}
