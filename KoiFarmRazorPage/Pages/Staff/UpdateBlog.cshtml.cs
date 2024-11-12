using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff;

public class UpdateBlog : PageModel
{
    [BindProperty]
    public Blog Blog { get; set; } = new Blog();
    
    
    private readonly IBlogRepository _blogRepository;

    public string Message { get; set; }

    public Dictionary<string,string> ValidateErrors { get; set; } = new Dictionary<string, string>();

    public UpdateBlog(IBlogRepository blogRepository)
    {
        this._blogRepository = blogRepository;
    }
    
    public void OnGet(long blogId)
    {
        Blog = _blogRepository.GetBlogByIdByStaff(blogId);
    }

    public IActionResult OnPost()
    {
        byte[] imageBytes = null;
        if (string.IsNullOrEmpty(Blog.Title))
        {
            ValidateErrors["BlogTitle"] = "Title khong duoc de trong";
        }else if (string.IsNullOrEmpty(Blog.Description))
        {
            ValidateErrors["BlogDescription"] = "Description khong duoc de trong";
        }
        else
        {

            Blog.BlogId = long.Parse(Request.Form["blogId"]);
            Blog.UpdateAt = DateTime.Now;
            Blog.IsDeleted = false;
            if (_blogRepository.UpdateBlog(Blog))
            {
                TempData["SuccessMessage"] = "Cap nhat blog thành công!!!";
                return RedirectToPage("/Staff/BlogManagement");
            }
            else
            {
                Blog = _blogRepository.GetBlogByIdByStaff(long.Parse(Request.Form["blogId"]));
                TempData["UpdateFail"] = "Cap nhat Blog khong thanh cong";
                return Page();
            }
        }
        Blog = _blogRepository.GetBlogByIdByStaff(long.Parse(Request.Form["blogId"]));
        return Page();
    }
}