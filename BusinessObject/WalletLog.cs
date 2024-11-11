using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class WalletLog
{
    public long WalletLogId { get; set; }

    public long? WalletId { get; set; }

    public double? Amount { get; set; }

    public string? Type { get; set; }
    // Deposit -> Nạp tiền vào ví
    // Pay -> Dùng tiền trong ví để thanh toán

    public DateTime? CreateAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
