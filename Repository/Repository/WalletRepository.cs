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
    public class WalletRepository : IWalletRepository
    {
        public void Refund(Wallet wallet)
        {
            WalletDAO.Refund(wallet);
        }

        public async Task<Wallet?> GetWalletById(long id)
            => await WalletDAO.GetWalletById(id);

        public async Task<Wallet> GetWalletByUserId(long userId)
            => WalletDAO.GetWalletByUserId(userId);


        public async Task<Wallet> GetWalletByUserEmail(string userEmail)
            => WalletDAO.GetWalletByUserEmail(userEmail);


        public void AddMoney(long userId, double money)
            => WalletDAO.AddMoney(userId, money);


        public void Deposit(long userId, double money)
            => WalletDAO.Deposit(userId, money);

        public void CreateWallet(Wallet wallet)
            => WalletDAO.CreateWallet(wallet);

        public void Update(Wallet wallet)
        => WalletDAO.Update(wallet);
    }
}