using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;
using Repository.Repository;

namespace KoiFarmRazorPage.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IUserRepository userRepository;
        private readonly IWalletRepository walletRepository;
        private readonly IHubContext<SignalRHub> hubContext;


        public CreateModel(IHubContext<SignalRHub> hubContext)
        {
            userRepository = new UserRepository();
            walletRepository = new WalletRepository();
            this.hubContext = hubContext;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty] public User User { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra xem email đã tồn tại hay chưa
            var existingUser = userRepository.GetUserByEmail(User.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email is already taken.");
                return Page();
            }

            User.CreateAt = DateTime.Now;
            User.IsDeleted = false;

            userRepository.SaveUser(User);

            Wallet wallet = new Wallet
            {
                UserId = User.UserId,
                Total = 0,
                LoyaltyPoint = 0,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                IsDeleted = false,
            };
            walletRepository.CreateWallet(wallet);

            await hubContext.Clients.All.SendAsync("RefreshData");

            return RedirectToPage("/Admin/Index");
        }
    }
}