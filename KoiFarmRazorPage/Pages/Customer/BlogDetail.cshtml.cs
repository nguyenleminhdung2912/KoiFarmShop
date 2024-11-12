using BusinessObject;
using DataAccessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Customer;

public class BlogDetail : PageModel
{
    private readonly IBlogRepository _blogRepository;
    private readonly BlogDAO blogDao = new BlogDAO();

    public BlogDetail(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public Blog? Blog { get; set; }

    public async Task OnGetAsync(int blogId)
    {
        Blog = await blogDao.GetBlogDetailForCustomer(blogId);
    }
}