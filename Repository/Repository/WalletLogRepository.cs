using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessObject;
using Repository.IRepository;

namespace Repository.Repository
{
    public class WalletLogRepository : IWalletLogRepository
    {
        public void CreateWalletLog(WalletLog walletLog)
            => WalletLogDAO.Create(walletLog);

        public Task<WalletLog?> GetWalletLogById(int id)
            => WalletLogDAO.GetWalletLogById(id);

        public WalletLog? GetWalletLogByType(string type)
            => WalletLogDAO.GetWalletLogByType(type);

        public Task<WalletLogDAO.WalletLogResponse> GetWalletLogsByWalletId(int pageIndex, int pageSize, long walletId)
            => WalletLogDAO.GetWalletLogsByWalletId(pageIndex, pageSize, walletId);
    }
}