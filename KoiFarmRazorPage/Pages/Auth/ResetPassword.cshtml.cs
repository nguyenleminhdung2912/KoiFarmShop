using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Auth;

public class ResetPassword : PageModel
{
    
    private readonly IUserRepository _userRepository;

    public ResetPassword(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Token { get; set; }

    [BindProperty]
    [Required, DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [BindProperty]
    [Required, Compare("NewPassword", ErrorMessage = "Passwords do not match"), DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    public IActionResult OnGet(string email, string token)
    {
        Email = email;
        Token = token;
        
        // Kiểm tra token (thời hạn hoặc mã hợp lệ)
        // Nếu cần, bạn có thể thêm mã logic để kiểm tra thời hạn token đã gửi.
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Gọi UserDAO để đặt lại mật khẩu
        var resetResult = await _userRepository.ResetPasswordAsync(Email, NewPassword);

        if (!resetResult)
        {
            ModelState.AddModelError(string.Empty, "Invalid request.");
            return Page();
        }

        // Thông báo cho người dùng đăng nhập lại với mật khẩu mới
        return RedirectToPage("/Auth/Login", new { Message = "Password has been reset successfully. Please log in with your new password." });
    }
}