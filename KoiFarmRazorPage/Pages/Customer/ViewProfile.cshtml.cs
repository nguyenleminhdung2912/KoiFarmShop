using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using KoiFarmRazorPage.Service;
using Repository.IRepository;
using Repository.Repository;

namespace KoiFarmRazorPage.Pages.Customer
{
    public class ViewProfileModel : PageModel
    {
        private readonly IUserRepository userRepository;
        private readonly IWalletRepository walletRepository;
        private readonly IVnPayService _vnPayService;

        public ViewProfileModel(IVnPayService vnPayService)
        {
            userRepository = new UserRepository();
            walletRepository = new WalletRepository();
            _vnPayService = vnPayService;
        }

        [BindProperty] public User UserProfile { get; set; }

        public Wallet UserWallet { get; set; } = default!;
        public List<WalletLog> WalletLogs { get; set; } = new List<WalletLog>();

        public async Task<IActionResult> OnGet(long UserId)
        {
            // Lấy Email lưu từ lúc login
            var Email = User.Identity.Name;
            
            // Lấy người dùng đang đăng nhập
            UserProfile = userRepository.GetUserByEmail(Email);
            
            // Ví người dùng
            UserWallet = await walletRepository.GetWalletByUserEmail(Email);
            WalletLogs = (List<WalletLog>)UserWallet.WalletLogs;

            return Page();
        }

        // Method to handle profile update
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                // Update thông tin profile
                userRepository.UpdateUser(UserProfile);
                
                // Tạo thông báo
                TempData["SuccessMessage"] = "Cập nhật thành công!";
                return RedirectToPage(); 
            }

            return Page();
        }

        public IActionResult OnPostPayment(long amount)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError(string.Empty, "Số tiền không hợp lệ.");
                return Page();
            }

            // Tạo URL thanh toán và chuyển hướng tới VNPay
            return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, amount));
        }
    }
}