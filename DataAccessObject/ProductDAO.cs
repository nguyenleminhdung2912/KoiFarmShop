﻿using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject
{
	public class ProductDAO
	{
		private static KoiFarmShopDatabaseContext _context = new KoiFarmShopDatabaseContext();
		public static List<Product> GetProductsByListString(string listString)
		{
			var list = new List<Product>();
			try
			{
				using var context = new KoiFarmShopDatabaseContext();
				var productIds = listString
						  .Split(',')
						  .Select(id => int.Parse(id.Trim()))
						  .ToList();
				list = context.Products
					.Where(pd => productIds.Contains((int)pd.ProductId))
					.ToList();
			}
			catch (Exception ex) { }
			return list;
		}


		public static List<Product> GetProducts()
		{
			return _context.Products.ToList();
		}

		public static Product GetProductById(long productId)
		{
			return _context.Products.FirstOrDefault(p => p.ProductId == productId);
		}

		public static List<Product> SearchProductByName(string name)
		{
			var products = _context.Products.Where(p => p.Name == name).Take(1);
			return products.ToList();	
		}
		public static void AddProduct(Product product)
		{
			_context.Products.Add(product);
			_context.SaveChanges();
		}

		public static void UpdateProduct(Product product)
		{
			var existProduct = _context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
			if (existProduct != null)
			{
				existProduct.Name = product.Name;
				existProduct.Type = product.Type;
				existProduct.ImageData = product.ImageData;
				existProduct.Price = product.Price;
				existProduct.UpdateAt = DateTime.Now;
				existProduct.Status = product.Status;
				existProduct.IsDeleted = product.IsDeleted;
				_context.Products.Update(existProduct);
				_context.SaveChanges() ;
			}
		}

		public static bool DeleteProduct(long productId)
		{
			var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);	
			if(product != null)
			{
				_context.Products.Remove(product);
				_context.SaveChanges();
				return true;
			}

			return false;
		}
	}
}