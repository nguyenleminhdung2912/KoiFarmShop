using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DataAccessObject
{
    public class UserDAO
    {
        public static User GetUserById(long? UserId)
        {
            using var db = new KoiFarmShopDatabaseContext();
            User returnuser
                = db.Users
                    .FirstOrDefault(c => c.UserId.Equals(UserId));
            return returnuser;
        }
        
        public static async Task<User?> GetUserByIdToDelete(long? userId)
        {
            using var db = new KoiFarmShopDatabaseContext();
            var returnUser
                = await db.Users
                    .Include(c => c.Consignments)
                    .Include(c => c.Orders)
                    .Include(c => c.Wallets)
                    .FirstOrDefaultAsync(c => c.UserId.Equals(userId));
            return returnUser;
        }

        public static User? CheckLogin(string email, string password)
        {
            // Kiểm tra xem người dùng có tồn tại không
            using var db = new KoiFarmShopDatabaseContext();
            var user = db.Users.SingleOrDefault(u => u.Email == email && u.Password == password);

            // Nếu tìm thấy người dùng, trả về dữ liệu (đăng nhập thành công)
            if (user != null)
            {
                return user;
            }

            // Nếu không tìm thấy người dùng, trả về null (đăng nhập thất bại)
            return null;
        }

        public static void DeleteUser(User user)
        {
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                var new1 =
                    context.Users.SingleOrDefault(c => c.UserId == user.UserId);
                context.Users.Remove(new1);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<User> GetUsers()
        {
            var list = new List<User>();
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                list = context.Users.ToList();
            }
            catch (Exception ex)
            {
            }

            return list;
        }

        public static void SaveUser(User user)
        {
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                var maxId = context.Users.Max(a => (int?)a.UserId) ?? 0;
                user.UserId = (short)(maxId + 1);
                context.Users.Add(user);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool UpdateUser(User user)
        {
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                var currentUser = context.Users.FirstOrDefault(u => u.UserId == user.UserId);

                if (currentUser == null)
                {
                    currentUser = context.Users.FirstOrDefault(u => u.Email == user.Email);
                }

                if (currentUser != null)
                {
                    if (!user.Name.IsNullOrEmpty())
                        currentUser.Name = user.Name;
                    if (!user.Email.IsNullOrEmpty())
                        currentUser.Email = user.Email;
                    if (!user.Phone.IsNullOrEmpty())
                        currentUser.Phone = user.Phone;
                    if (!user.Role.IsNullOrEmpty())
                        currentUser.Role = user.Role;
                    if (!user.Password.IsNullOrEmpty())
                        currentUser.Password = user.Password;
                    currentUser.IsDeleted = user.IsDeleted;
                    context.Users.Update(currentUser);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static User GetUserByEmail(string userEmail)
        {
            using var db = new KoiFarmShopDatabaseContext();
            User returnUser = db.Users
                .FirstOrDefault(c => c.Email.Equals(userEmail));
            return returnUser;
        }

        public static void Register(User user)
        {
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                var maxId = context.Users.Max(a => (int?)a.UserId) ?? 0;
                user.UserId = (short)(maxId + 1);
                context.Users.Add(user);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Phương thức đặt lại mật khẩu
        public static async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            using var _context = new KoiFarmShopDatabaseContext();

            // Tìm người dùng theo email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                // Người dùng không tồn tại
                return false;
            }

            // Cập nhật mật khẩu mới
            user.Password = newPassword;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}