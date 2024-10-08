using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Wallet
{
    public long WalletId { get; set; }

    public long? UserId { get; set; }

    public double? Total { get; set; }

    public int? LoyaltyPoint { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<WalletLog> WalletLogs { get; set; } = new List<WalletLog>();
}
