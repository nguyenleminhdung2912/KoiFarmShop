using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject
{
    public class WalletLogDAO
    {
        public static void Create(WalletLog walletLog)
        {
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                var maxId = context.WalletLogs.Max(a => (int?)a.WalletLogId) ?? 0;
                walletLog.WalletLogId = (short)(maxId + 1);
                context.WalletLogs.Add(walletLog);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<WalletLog?> GetWalletLogById(int id)
        {
            using var db = new KoiFarmShopDatabaseContext();
            WalletLog? returnWalletLog
                = db.WalletLogs
                    .FirstOrDefault(c => c.WalletLogId.Equals(id));
            return returnWalletLog;
        }

        public static WalletLog? GetWalletLogByType(string type)
        {
            using var db = new KoiFarmShopDatabaseContext();
            WalletLog? returnWalletLog
                = db.WalletLogs
                    .FirstOrDefault(c => c.Type.Equals(type));
            return returnWalletLog;
        }
    }
}
