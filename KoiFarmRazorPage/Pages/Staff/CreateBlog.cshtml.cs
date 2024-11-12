using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff;

public class CreateBlog : PageModel
{
    [BindProperty] public Blog Blog { get; set; }


    public String Message { get; set; }

    public Dictionary<string, string> ValidateErrors { get; set; } = new Dictionary<string, string>();

    private readonly IBlogRepository _blogRepository;

    public CreateBlog(IBlogRepository blogRepository)
    {
        this._blogRepository = blogRepository;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(Blog.Title))
        {
            ValidateErrors["BlogTitle"] = "Title khong duoc de trong";
        }
        else if (string.IsNullOrEmpty(Blog.Description))
        {
            ValidateErrors["BlogDescription"] = "Description khong duoc de trong";
        }
        else
        {
            Blog.BlogId = GetBlogId();
            // if (!User.Identity.IsAuthenticated)
            // {
            //     return RedirectToPage("/Customer/Index");
            // }
            // else
            // {
            //     Blog.UserId = long.Parse(User.FindFirst("userId").Value);
            // }
            Blog.UserId = long.Parse(User.FindFirst("userId").Value);
            Blog.CreateAt = DateTime.Now;
            Blog.IsDeleted = false;
            if (_blogRepository.AddBlog(Blog))
            {
                TempData["SuccessMessage"] = "Tao blog thành công!!!";
                return RedirectToPage("/Staff/BlogManagement");
            }
            else
            {
                TempData["CreateFail"] = "Tạo blog không thành công!!!";
                return Page();
            }
        }

        return Page();
    }

    private long GetBlogId()
    {
        return _blogRepository.GetNextBlogId();
    }
}