using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject
{
    public class UserDAO
    {
        public static User GetUserById(short UserId)
        {
            using var db = new KoiFarmShopDatabaseContext();
            User returnuser
                = db.Users
                .FirstOrDefault(c => c.UserId.Equals(UserId));
            return returnuser;
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
            catch (Exception ex) { }
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

        public static void UpdateUser(User user)
        {
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                context.Entry<User>(user).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static User GetUserByEmail(string userEmail)
        {
            using var db = new KoiFarmShopDatabaseContext();
            User returnUser = db.Users.FirstOrDefault(c => c.Email.Equals(userEmail));
            return returnUser;
        }
    }
}
