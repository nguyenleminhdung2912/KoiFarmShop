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
        
        public class WalletLogResponse
        {
            public List<WalletLog> WalletLogs { get; set; }
            public int TotalPages { get; set; }
            public int PageIndex { get; set; }
        }

        public static async Task<WalletLogResponse> GetWalletLogsByWalletId(int pageIndex, int pageSize, long walletId)
        {
            using var _context = new KoiFarmShopDatabaseContext(); 

            var query = _context.WalletLogs.AsQueryable();
            query = query.Where(x => x.WalletId == walletId).OrderByDescending(x => x.CreateAt);

            int count = await query.CountAsync(); 
            int totalPages = (int)Math.Ceiling(count / (double)pageSize); 

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            // Gán dữ liệu vào ViewModel
            return new WalletLogResponse
            {
                WalletLogs = await query.ToListAsync(),
                TotalPages = totalPages,
                PageIndex = pageIndex
            };
        }
    }
}
