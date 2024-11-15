using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Scripting;
using Repository.IRepository;
using Repository.Repository;
using System.ComponentModel.DataAnnotations;
using KoiFarmRazorPage.Service;

namespace KoiFarmRazorPage.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IUserRepository userRepository;
        private readonly IWalletRepository walletRepository;
        public readonly EmailService _emailService;

        public RegisterModel(EmailService emailService)
        {
            userRepository = new UserRepository();
            walletRepository = new WalletRepository();
            _emailService = emailService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [EmailAddress]
            [RegularExpression(@"^[^@\s]+@gmail\.com$", ErrorMessage = "Email must end with @gmail.com.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra xem email đã tồn tại hay chưa
            var existingUser = userRepository.GetUserByEmail(Input.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email is already taken.");
                return Page();
            }

            // Tạo người dùng mới
            var user = new User
            {
                Name = Input.Email,
                Email = Input.Email,
                Password = Input.Password, // Mã hóa mật khẩu
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                Role = "Customer",
                IsDeleted = false
            };
            // Lưu người dùng vào database
            userRepository.Register(user);
            
            Wallet wallet = new Wallet
            {
                UserId = user.UserId,
                Total = 0,
                LoyaltyPoint = 0,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                IsDeleted = false,
            };
            walletRepository.CreateWallet(wallet);

            var subject = "Chào mừng bạn đến với Koi Farm Shop!";
            var loginUrl = Url.Page("/Auth/Login", new { area = "" }.ToString(), null, Request.Scheme);
            var body = $@"
            <p>Chào {user.Name},</p>
            <p>Cảm ơn bạn đã đăng ký tài khoản tại Koi Farm Shop. Tài khoản của bạn đã được tạo thành công.</p>
            <p>Để bắt đầu trải nghiệm, hãy bấm vào nút bên dưới để đăng nhập vào hệ thống của chúng tôi.</p>
            <p><a href='{loginUrl}' style='display: inline-block; padding: 10px 20px; background-color: #4CAF50; color: white; text-decoration: none; border-radius: 5px; font-weight: bold;'>Bấm vào đây để đăng nhập</a></p>
            <p>Chúc bạn có một trải nghiệm tuyệt vời tại KoiFarm!</p>
            <p>Trân trọng,<br/>Đội ngũ KoiFarm</p>
        ";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            // Đăng nhập người dùng hoặc chuyển hướng đến trang đăng ký thành công
            return RedirectToPage("/Auth/RegisterSuccess");
        }
    }
}
