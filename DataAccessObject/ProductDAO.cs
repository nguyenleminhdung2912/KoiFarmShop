using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject
{
    public class ProductDAO
    {

		public static List<Product> GetProducts()
		{
			using var db = new KoiFarmShopDatabaseContext();
			return db.Products.ToList();
		}

		public static Product SearchProductByName(string productName)
		{
			using var db = new KoiFarmShopDatabaseContext();
			var product = db.Products.FirstOrDefault(p => p.Name == productName);
			return product;
		}
		

		public static void AddProduct(Product product)
		{
			using var db = new KoiFarmShopDatabaseContext();
			db.Products.Add(product);
			db.SaveChanges();
		}

		public static void UpdateProduct(Product product)
		{
			using var db = new KoiFarmShopDatabaseContext();
			db.Products.Update(product);
			db.SaveChanges();
		}

		public static void DeleteProduct(long productId)
		{
			using var db = new KoiFarmShopDatabaseContext();
			var existProduct = db.Products.Include(p => p.ProductRatings).FirstOrDefault(p => p.ProductId == productId);
			if(existProduct != null)
			{
				db.Products.Remove(existProduct);
				db.SaveChanges();
			}

		}
	}
}
