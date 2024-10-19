using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject
{
    public class ProductRatingDAO
    {
        public static List<ProductRating> GetProductRatings()
        {
            using var db = new KoiFarmShopDatabaseContext();
            return db.ProductRatings.ToList();
        }

        public static void AddProductRating(ProductRating productRating)
        {
			using var db = new KoiFarmShopDatabaseContext();
            db.ProductRatings.Add(productRating);
            db.SaveChanges();
		}

		public static void UpdateProductRating(ProductRating productRating)
		{
			using var db = new KoiFarmShopDatabaseContext();
			db.ProductRatings.Update(productRating);
			db.SaveChanges();
		}

		public static void DeleteProductRating(long productRatingId)
		{
			using var db = new KoiFarmShopDatabaseContext();
			var existProductRating = db.ProductRatings.FirstOrDefault(p => p.ProductRatingId == productRatingId);
			if (existProductRating != null)
			{
				db.ProductRatings.Remove(existProductRating);
				db.SaveChanges();
			}

		}
	}
}
