using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff;
[Authorize(Roles = "Staff")]

public class UpdateBlog : PageModel
{
    [BindProperty]
    public Blog Blog { get; set; } = new Blog();
    
    
    private readonly IBlogRepository _blogRepository;
    private readonly IHubContext<SignalRHub> hubContext;


    public string Message { get; set; }

    public Dictionary<string, string> ValidateErrors { get; set; } = new Dictionary<string, string>();

    public UpdateBlog(IBlogRepository blogRepository, IHubContext<SignalRHub> hubContext)
    {
        this._blogRepository = blogRepository;
        this.hubContext = hubContext;
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
            ValidateErrors["BlogTitle"] = "Tiêu đề không được để trônống";
        }else if (string.IsNullOrEmpty(Blog.Description))
        {
            ValidateErrors["BlogDescription"] = "Nội dung không được để trống";
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