﻿using BusinessObject;
using DataAccessObject;
using Repository.IRepository;

namespace Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        public User CheckLogin(string email, string password)
            => UserDAO.CheckLogin(email, password);

        public void DeleteUser(User User)
            => UserDAO.DeleteUser(User);

        public User GetUserByEmail(string UserEmail)
            => UserDAO.GetUserByEmail(UserEmail);

        public User GetUserById(long? UserId)
            => UserDAO.GetUserById(UserId);
        
        public async Task<User?> GetUserByIdToDelete(long? userId)
            => await UserDAO.GetUserByIdToDelete(userId);

        public List<User> GetUsers()
            => UserDAO.GetUsers();

        public void Register(User user)
            => UserDAO.Register(user);

        public void SaveUser(User User)
            => UserDAO.SaveUser(User);

        public Task<bool> ResetPasswordAsync(string email, string newPassword)
            => UserDAO.ResetPasswordAsync(email, newPassword);

        public void UpdateUser(User User)
            => UserDAO.UpdateUser(User);
    }
}