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

namespace KoiFarmRazorPage.Pages.Customer
{
    public class UpdateProfileModel : PageModel
    {
        private readonly IUserRepository userRepository;

        public UpdateProfileModel(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [BindProperty]
        public User UserProfile { get; set; }

        public IActionResult OnGet(long UserId)
        {
            var Email = User.Identity.Name;
            // G?i ph??ng th?c GetUserById ?? l?y thông tin ng??i dùng t? UserRepository
            UserProfile = userRepository.GetUserByEmail(Email);

            return Page();
        }
        public IActionResult OnPost(long UserId) 
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var Email = User.Identity.Name;
            var userToUpdate = userRepository.GetUserByEmail(Email);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            // C?p nh?t thông tin ng??i dùng
            userToUpdate.Name = UserProfile.Name;
            userToUpdate.Phone = UserProfile.Phone;

             userRepository.UpdateUser(userToUpdate);

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToPage("/Customer/ViewProfile");
        }
    }
}
