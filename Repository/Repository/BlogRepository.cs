using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessObject;
using Repository.IRepository;

namespace Repository.Repository
{
    public class BlogRepository : IBlogRepository
    {
        public List<Blog> GetBlogsForStaff()
        {
            return BlogDAO.GetAllBlogsForStaff();
        }

        public long GetNextBlogId()
        {
            return BlogDAO.GetNextBlogId();
        }

        public bool AddBlog(Blog blog)
        {
            return BlogDAO.AddBlog(blog);
        }

        public bool DeleteBlogById(long id)
        {
            return BlogDAO.DeleteBlogById(id);
        }
        
        public Blog GetBlogByIdByStaff(long id)
        {
            return BlogDAO.GetBlogByIdByStaff(id);
        }

        public bool UpdateBlog(Blog blog)
        {
            return BlogDAO.UpdateBlog(blog);
        }
    }
}
