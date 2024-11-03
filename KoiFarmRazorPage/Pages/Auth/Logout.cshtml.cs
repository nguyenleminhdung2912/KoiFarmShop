using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KoiFarmRazorPage.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public async Task OnGet()
        {
            // Đăng xuất người dùng
            await HttpContext.SignOutAsync();

            // Chuyển hướng về trang đăng nhập hoặc trang chủ
            Response.Redirect("/Customer/Index");
        }
    }
}
