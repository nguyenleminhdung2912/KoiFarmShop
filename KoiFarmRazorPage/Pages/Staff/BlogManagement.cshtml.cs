using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using NguyenLeMinhDungFall2024RazorPages;
using NuGet.Protocol.Plugins;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff;
[Authorize(Roles = "Staff")]

public class BlogManagement : PageModel
{
    public string Message { get; set; }
    public List<Blog> Blogs { get; set; } = new List<Blog>();

    private readonly IBlogRepository _blogRepository;
    private readonly IHubContext<SignalRHub> hubContext;


    public BlogManagement(IBlogRepository blogRepository, IHubContext<SignalRHub> hubContext)
    {
        this._blogRepository = blogRepository;
        this.hubContext = hubContext;
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

        if (handler == "Search")
        {
            if (string.IsNullOrEmpty(Request.Form["blogTitle"]))
            {
                TempData["SearchFail"] = "Blog with this title does not exits";
                Blogs = _blogRepository.GetBlogsForStaff();
            }
            else
            {
                if (_blogRepository.GetBlogByTitle(Request.Form["blogTitle"]).IsNullOrEmpty())
                {
                    TempData["SearchFail"] = "Blog with this title does not exits";
                    Blogs = _blogRepository.GetBlogByTitle(Request.Form["blogTitle"]);
                }
                else
                {
                    Blogs = _blogRepository.GetBlogByTitle(Request.Form["blogTitle"]);
                }
            }
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
                return RedirectToPage("/Staff/UpdateBlog", new { blogId = long.Parse(Request.Form["blogId"]) });
            }
        }

        return Page();
    }
}