using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject
{
    public class ProductDAO
    {
        private static KoiFarmShopDatabaseContext _context = new KoiFarmShopDatabaseContext();

        public static async Task<List<Product>> GetProductsForCustomer()
        {
            List<Product> products = new List<Product>();
            var context = new KoiFarmShopDatabaseContext();
            products = await context.Products.Where(c => c.Status == "Available" && c.IsDeleted == false).ToListAsync();
            return products;
        }

        public static List<Product> GetProductsByListString(string listString)
        {
            var list = new List<Product>();
            try
            {
                using var context = new KoiFarmShopDatabaseContext();

                // Chia chuỗi đầu vào thành danh sách các ID, giữ nguyên các ID trùng lặp
                var productIds = listString
                    .Split(',')
                    .Select(id => int.Parse(id.Trim()))
                    .ToList();

                // Lấy các sản phẩm từ cơ sở dữ liệu
                list = productIds
                    .Select(id =>
                        context.Products.FirstOrDefault(pd => pd.ProductId == id)) // Lấy từng sản phẩm theo từng ID
                    .Where(pd => pd != null) // Loại bỏ null (nếu có)
                    .ToList();
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu có
                Console.WriteLine(ex.Message);
            }

            return list;
        }

        public static Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        public static List<Product> GetProducts()
        {
            return _context.Products.Where(k => k.IsDeleted == false).ToList();
        }

        public static Product GetProductById(long productId)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == productId);
        }

        public static List<Product> SearchProductByName(string name)
        {
            var products = _context.Products.Where(p => p.Name == name && p.IsDeleted == false).Take(1);
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
                if (product.ImageData != null)
                {
                    existProduct.ImageData = product.ImageData;
                }

                existProduct.Price = product.Price;
                existProduct.UpdateAt = DateTime.Now;
                existProduct.Status = product.Status;
                existProduct.IsDeleted = product.IsDeleted;
                existProduct.Quantity = product.Quantity;
                _context.Products.Update(existProduct);
                _context.SaveChanges();
            }
        }

        public static bool DeleteProduct(long productId)
        {
            var product = _context.Products.Include(p => p.ProductRatings)
                .FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                if (product.ProductRatings.Any())
                {
                    product.ProductRatings.Clear();
                    product.IsDeleted = true;
                    _context.Products.Update(product);
                    // _context.Products.Remove(product);
                    _context.SaveChanges();
                    return true;
                }

                _context.Products.Remove(product);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public static long GetNextConsignmentId()
        {
            // Lấy ID lớn nhất hiện có trong bảng Product
            var maxId = _context.Products.Max(p => (long?)p.ProductId) ?? 0;
            return maxId + 1; // Trả về ID mới
        }
    }
}