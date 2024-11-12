using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject
{
    public class BlogDAO
    {
        private static KoiFarmShopDatabaseContext _context = new KoiFarmShopDatabaseContext();

        public static List<Blog> GetAllBlogsForStaff()
        {
            return _context.Blogs.Include(b => b.User).ToList();
        }
        
        public static long GetNextBlogId()
        {
            // Lấy ID lớn nhất hiện có trong bảng Consignment
            var maxId = _context.Blogs.Max(b => (long?)b.BlogId) ?? 0;
            return maxId + 1; // Trả về ID mới
        }

        public static bool AddBlog(Blog blog)
        {
            _context.Blogs.Add(blog);
            _context.SaveChanges();
            return true;
        }

        public static bool DeleteBlogById(long id)
        {
            var existingBlog = _context.Blogs.Find(id);
            if (existingBlog != null)
            {
                _context.Blogs.Remove(existingBlog);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public static bool UpdateBlog(Blog blog)
        {
            var existingBlog = _context.Blogs.Find(blog.BlogId);
            if (existingBlog != null)
            {
                existingBlog.Title = blog.Title;
                existingBlog.Description = blog.Description;
                existingBlog.UpdateAt = blog.UpdateAt;
                // existingBlog.ImageData = blog.ImageData;
                existingBlog.IsDeleted = blog.IsDeleted;
                existingBlog.UserId = blog.UserId;
                _context.Blogs.Update(existingBlog);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public static Blog GetBlogByIdByStaff(long id)
        {
            return _context.Blogs.Include(b => b.User).FirstOrDefault(b => b.BlogId == id);
        }
    }
}
