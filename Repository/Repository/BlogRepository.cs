using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class BlogRepository
    {
		public static List<Blog> GetProductRatings()
		{
			using var db = new KoiFarmShopDatabaseContext();
			return db.Blogs.ToList();
		}

		public static void AddBlog(Blog blog)
		{
			using var db = new KoiFarmShopDatabaseContext();
			db.Blogs.Add(blog);
			db.SaveChanges();
		}

		public static void UpdateBlog(Blog blog)
		{
			using var db = new KoiFarmShopDatabaseContext();
			db.Blogs.Update(blog);
			db.SaveChanges();
		}

		public static void DeleteBlog(long blogId)
		{
			using var db = new KoiFarmShopDatabaseContext();
			var existBlog = db.Blogs.FirstOrDefault(b => b.BlogId == blogId);
			if (existBlog != null)
			{
				db.Blogs.Remove(existBlog);
				db.SaveChanges();
			}

		}
	}
}
