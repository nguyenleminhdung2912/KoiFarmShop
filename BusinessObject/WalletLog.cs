using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class WalletLog
{
    public long WalletLogId { get; set; }

    public long? WalletId { get; set; }

    public double? Amount { get; set; }

    public string? Type { get; set; }

    public DateTime? CreateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
