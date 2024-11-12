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
            return _context.Blogs.Include(b => b.User).OrderByDescending(b => b.CreateAt).ToList();
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
                existingBlog.IsDeleted = blog.IsDeleted;
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

        public class BlogResponse
        {
            public List<Blog> Blogs { get; set; }
            public int TotalPages { get; set; }
            public int PageIndex { get; set; }
        }

        public static async Task<BlogResponse> GetBlogsForCustomer(string searchTerm, int pageIndex, int pageSize)
        {
            var query = _context.Blogs.Include(x => x.User).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Title.ToLower().Contains(searchTerm.ToLower())
                                         && x.IsDeleted == false);
            }

            int count = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new BlogResponse
            {
                Blogs = await query.ToListAsync(),
                TotalPages = totalPages,
                PageIndex = pageIndex
            };
        }

        public static List<Blog> SearchBlogByName(string title)
        {
            return _context.Blogs.Include(x => x.User).Where(b => b.Title.ToLower().Contains(title.ToLower())).Take(1).ToList();
        }
        public async Task<Blog?> GetBlogDetailForCustomer(long id)
        {
            return await _context.Blogs.Include(b => b.User).FirstOrDefaultAsync(b => b.BlogId == id);
        }
    }
}