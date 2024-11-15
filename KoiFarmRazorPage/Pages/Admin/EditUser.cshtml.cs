using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;
using Repository.Repository;

namespace KoiFarmRazorPage.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IUserRepository userRepository;
        private readonly IHubContext<SignalRHub> hubContext;


        public EditModel(IHubContext<SignalRHub> hubContext)
        {
            userRepository = new UserRepository();
            this.hubContext = hubContext;
        }

        [BindProperty] public User User { get; set; } = default!;


        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [BindProperty]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            User = user;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Xem chi tiết lỗi trong log
                }

                return Page();
            }

            try
            {
                var existingUser = userRepository.GetUserById(User.UserId);
                if (existingUser == null)
                {
                    return NotFound();
                }

                if (Password != null)
                {
                    User.Password = Password;
                }

                User.UpdateAt = DateTime.Now;
                User.CreateAt = existingUser.CreateAt;
                User.IsDeleted = existingUser.IsDeleted;


                userRepository.UpdateUser(User);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(User.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            hubContext.Clients.All.SendAsync("RefreshData");
            
            TempData["UpdateSuccess"] = "Update account successfully.";

            return RedirectToPage("./Index");
        }

        private bool UserExists(long id)
        {
            return userRepository.GetUserById(id) != null;
        }
    }
}