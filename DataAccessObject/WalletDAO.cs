using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject
{
    public class WalletDAO
    {
        public static void Refund(Wallet wallet)
        {
            using var context = new KoiFarmShopDatabaseContext();
            var existWallet = context.Wallets.Include(w => w.User).FirstOrDefault(w => w.WalletId == wallet.WalletId);
            if (existWallet != null)
            {
                existWallet.Total = wallet.Total;
                context.Wallets.Update(existWallet);
                context.SaveChanges();
            }
        }
        public static async Task<Wallet?> GetWalletById(long id)
        {
            using var db = new KoiFarmShopDatabaseContext();
            Wallet? returnWallet
                = db.Wallets
                    .Include(w => w.WalletLogs)
                    .FirstOrDefault(c => c.WalletId.Equals(id));
            return returnWallet;
        }

        public static Wallet GetWalletByUserId(long userId)
        {
            try
            {
                using var db = new KoiFarmShopDatabaseContext();

                //Get Wallet
                var wallet = db.Wallets
                    .Include(w => w.WalletLogs)
                    .FirstOrDefault(w => w.UserId.Equals(userId));
                return wallet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Wallet GetWalletByUserEmail(string userEmail)
        {
            try
            {
                using var db = new KoiFarmShopDatabaseContext();
                //Get User
                var user = db.Users.FirstOrDefault(u => u.Email != null && u.Email.Equals(userEmail));

                //Get Wallet
                var wallet = db.Wallets
                    .FirstOrDefault(w => w.UserId.Equals(user.UserId));
                return wallet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void AddMoney(long userId, double money)
        {
            throw new NotImplementedException();
        }

        public static void Deposit(long userId, double money)
        {
            throw new NotImplementedException();
        }

        public static void CreateWallet(Wallet wallet)
        {
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                var maxId = context.Wallets.Max(a => (int?)a.WalletId) ?? 0;
                wallet.WalletId = (short)(maxId + 1);
                context.Wallets.Add(wallet);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void Update(Wallet wallet)
        {
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                Wallet? curentWallet = context.Wallets.FirstOrDefault(w => w.WalletId.Equals(wallet.WalletId));
                curentWallet.Total = wallet.Total;
                context.Wallets.Update(curentWallet);

                context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}