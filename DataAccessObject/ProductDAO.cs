using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject
{
    public class ProductDAO
    {
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
    }
}
