using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Cart
    {
        public List<KoiFish> KoiFishes { get; set; } = new List<KoiFish>();
        public List<Product> Products { get; set; } = new List<Product>();

        public void AddKoiFish(KoiFish koiFish)
        {
            // Kiểm tra xem cá Koi đã có trong giỏ hàng chưa
            if (!KoiFishes.Any(k => k.KoiFishId == koiFish.KoiFishId))
            {
                KoiFishes.Add(koiFish); // Thêm cá Koi vào giỏ hàng
            }
        }

        public void AddProduct(Product product)
        {
            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            if (!Products.Any(p => p.ProductId == product.ProductId))
            {
                Products.Add(product); // Thêm sản phẩm vào giỏ hàng
            }
        }

        public decimal CalculateTotal()
        {
            decimal total = 0;
            total += KoiFishes.Sum(k => (decimal)(k.Price ?? 0)); 
            total += Products.Sum(p => (decimal)(p.Price ?? 0));
            return total;
        }
    }

}
