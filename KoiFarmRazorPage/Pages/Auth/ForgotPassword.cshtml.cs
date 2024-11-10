using System.ComponentModel.DataAnnotations;
using KoiFarmRazorPage.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Auth;

public class ForgotPassword : PageModel
{
    public readonly EmailService _emailService;
    private readonly IUserRepository _userRepository;

    public ForgotPassword(EmailService emailService, IUserRepository userRepository)
    {
        _emailService = emailService;
        _userRepository = userRepository;
    }
    
    [BindProperty]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    
    [TempData]
    public string? Message { get; set; }
    
    public void OnGet()
    {
        
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var user = _userRepository.GetUserByEmail(Email);
        if (user == null)
        {
            throw new InvalidOperationException("User with this email does not exist.");
        }

        // Tạo mã token đặt lại mật khẩu (ở đây dùng GUID)
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var resetLink = Url.Page("/Auth/ResetPassword", null, new { email = user.Email, token = token }, Request.Scheme);

        // Send email with reset link
        var subject = "Password Reset Request";
        var body = $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";
        await _emailService.SendEmailAsync(Email, subject, body);

        // Notify the user to check their email
        Message = "A password reset link has been sent to your email address.";
        return RedirectToPage("/Auth/ForgotPassword", new { Message });
        // return Page(new {Message = "Password reset link has been sent to your email address."});
    }
}