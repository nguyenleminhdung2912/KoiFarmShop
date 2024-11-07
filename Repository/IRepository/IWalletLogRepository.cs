using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository.IRepository
{
    public interface IWalletLogRepository
    {
        void CreateWalletLog(WalletLog walletLog);
        
        Task<WalletLog?> GetWalletLogById(int id);
        
        WalletLog? GetWalletLogByType(string type);
    }
}
