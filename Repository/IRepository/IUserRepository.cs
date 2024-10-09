using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IUserRepository
    {
        User GetUserById(short UserId);
        User GetUserByEmail(string UserEmail);
        User CheckLogin(string email, string password);
        List<User> GetUsers();
        void UpdateUser(User User);
        void DeleteUser(User User);
        void SaveUser(User User);
    }
}
