using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository.IRepository
{
    public interface IBlogRepository
    {
        List<Blog> GetBlogsForStaff();
        
        long GetNextBlogId();
        
        bool AddBlog(Blog blog);
        
        bool DeleteBlogById(long id);
        
        Blog GetBlogByIdByStaff(long id);
        
        bool UpdateBlog(Blog blog);
    }
}
