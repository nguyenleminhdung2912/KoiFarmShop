using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace KoiFarmRazorPage.Pages.Auth
{
    public class LoginModel : PageModel
    {
        public IConfigurationRoot Configuration { get; set; }

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

            // Kiểm tra email và mật khẩu
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

            // Nếu xác thực thất bại
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
