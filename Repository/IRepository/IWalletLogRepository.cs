using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessObject;

namespace Repository.IRepository
{
    public interface IWalletLogRepository
    {
        void CreateWalletLog(WalletLog walletLog);
        
        Task<WalletLog?> GetWalletLogById(int id);
        
        WalletLog? GetWalletLogByType(string type);
        
        Task<WalletLogDAO.WalletLogResponse> GetWalletLogsByWalletId(int pageIndex, int pageSize, long walletId);
    }
}
