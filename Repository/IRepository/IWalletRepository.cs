using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository.IRepository
{
    public interface IWalletRepository
    {
        Task<Wallet?> GetWalletById(long id);
        Task<Wallet> GetWalletByUserId(long userId);
        Task<Wallet> GetWalletByUserEmail(string userEmail);
        void AddMoney(long userId, double money);
        void Deposit(long userId, double money);
        void CreateWallet(Wallet wallet);
        void Update(Wallet wallet);
    }
}
