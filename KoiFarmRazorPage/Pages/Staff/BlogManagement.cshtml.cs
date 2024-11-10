using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol.Plugins;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff;

public class BlogManagement : PageModel
{
    public string Message { get; set; }
    public List<Blog> Blogs { get; set; } = new List<Blog>();

    private readonly IBlogRepository _blogRepository;

    public BlogManagement(IBlogRepository blogRepository)
    {
        this._blogRepository = blogRepository;
    }
    public void OnGet()
    {
        Blogs = _blogRepository.GetBlogsForStaff();
    }

    public IActionResult OnPost()
    {
        string handler = Request.Form["handler"];
        if (handler == "Create")
        {
            return RedirectToPage("/Staff/CreateBlog");
        }

        if (handler == "Delete")
        {
            if (string.IsNullOrEmpty(Request.Form["blogId"]))
            {
                Message = "Chon blog cu the de xoa";
                Blogs = _blogRepository.GetBlogsForStaff();
            }
            else
            {
                if (_blogRepository.DeleteBlogById(long.Parse(Request.Form["blogId"])))
                {
                    TempData["SuccessMessage"] = "Xoa blog thành công!!!";
                    Blogs = _blogRepository.GetBlogsForStaff();
                }
                else
                {
                    Message = "Xoa blog khong thanh cong!!";
                    Blogs = _blogRepository.GetBlogsForStaff();
                }
            }
        }

        if (handler == "Update")
        {
            
            if (string.IsNullOrEmpty(Request.Form["blogId"]))
            {
                Message = "Chon blog cu the de update";
                Blogs = _blogRepository.GetBlogsForStaff();
            }
            else
            {
               return RedirectToPage("/Staff/UpdateBlog", new {blogId = long.Parse(Request.Form["blogId"])});
            }
        }
        return Page();
    }
}