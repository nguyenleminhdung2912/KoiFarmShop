using BusinessObject;
using DataAccessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Customer;

public class ViewBlog : PageModel
{
    private readonly IBlogRepository _blogRepository;

    public ViewBlog(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public IList<Blog> Blogs { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public string searchTerm { get; set; }

    //paging
    public int PageIndex { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 2;

    public async Task OnGetAsync(int pageIndex = 1)
    {
        var result = await _blogRepository.GetBlogsForCustomer(searchTerm, pageIndex, 3);

        Blogs = result.Blogs;
        PageIndex = result.PageIndex;
        TotalPages = result.TotalPages;
    }
}