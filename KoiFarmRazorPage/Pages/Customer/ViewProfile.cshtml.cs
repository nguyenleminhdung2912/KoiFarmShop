using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccessObject;
using Repository.IRepository;
using Repository.Repository;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace KoiFarmRazorPage.Pages.Customer
{
    public class ViewProfileModel : PageModel
    {

       private readonly IUserRepository userRepository;

        public ViewProfileModel(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User UserProfile { get; set; }

        public IActionResult OnGet(long UserId)
        {
            var Email = User.Identity.Name;
            // Gọi phương thức GetUserById để lấy thông tin người dùng từ UserRepository
            UserProfile = userRepository.GetUserByEmail(Email);

            return Page(); 
        }
    }

}
