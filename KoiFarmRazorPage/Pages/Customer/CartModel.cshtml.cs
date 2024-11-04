using BusinessObject;
using DataAccessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace KoiFarmRazorPage.Pages.Customer
{
    public class CartModelModel : PageModel
    {
        public Cart UserCart { get; set; }
       public void OnGet()
        {
            // T?o m?i gi? hàng n?u ch?a có
            if (HttpContext.Session.GetString("UserCart") == null)
            {
                UserCart = new Cart();
                HttpContext.Session.SetString("UserCart", JsonConvert.SerializeObject(UserCart));
            }
            else
            {
                // L?y gi? hàng t? session
                string cartJson = HttpContext.Session.GetString("UserCart");
                UserCart = JsonConvert.DeserializeObject<Cart>(cartJson);
            }
        }

        public IActionResult OnPostAddKoiFish(long koiFishId)
        {
            // L?y cá Koi t? DAO
            var koiFish = KoiFishDAO.GetKoiFishById(koiFishId);
            if (koiFish != null)
            {
                // Thêm cá Koi vào gi? hàng
                UserCart.AddKoiFish(koiFish);
                UpdateCartInSession();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostAddProduct(long productId)
        {
            // L?y s?n ph?m t? DAO
            var product = ProductDAO.GetProductById(productId);
            if (product != null)
            {
                // Thêm s?n ph?m vào gi? hàng
                UserCart.AddProduct(product);
                UpdateCartInSession();
            }

            return RedirectToPage();
        }

        private void UpdateCartInSession()
        {
            // C?p nh?t gi? hàng vào session
            HttpContext.Session.SetString("UserCart", JsonConvert.SerializeObject(UserCart));
        }
    }
}
