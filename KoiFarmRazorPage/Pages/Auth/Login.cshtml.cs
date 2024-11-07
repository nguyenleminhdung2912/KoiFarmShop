using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Repository.IRepository;
using Repository.Repository;

namespace KoiFarmRazorPage.Pages.Auth
{
    public class LoginModel : PageModel
    {
        public IConfigurationRoot Configuration { get; set; }
        public IUserRepository userRepository;

        public LoginModel()
        {
            userRepository = new UserRepository();
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

            var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = config.Build();
            var adminUser = Configuration["AdminAccount:Email"];
            var adminPassword = Configuration["AdminAccount:Password"];

            // Kiểm tra email và mật khẩu có phải admin không
            if (Email == adminUser && Password == adminPassword)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Xác thực và tạo cookie đăng nhập
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Chuyển hướng đến trang Index
                return RedirectToPage("/Admin/Index");
            }

            // Nếu không phải tài khoản admin, kiểm tra tài khoản người dùng
            var user = userRepository.CheckLogin(Email, Password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("userId", user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role) // Lấy Role từ đối tượng User
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Xác thực và tạo cookie đăng nhập
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Chuyển hướng dựa trên Role
                if (user.Role == "Admin")
                {
                    return RedirectToPage("/Admin/Index");
                }
                else if (user.Role == "Staff")
                {
                    return RedirectToPage("/Staff/StaffPage");
                }
                else if (user.Role == "Customer")
                {
                    return RedirectToPage("/Customer/Index");
                }
            }

            // Nếu xác thực thất bại
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
