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
            // T?o m?i gi? h�ng n?u ch?a c�
            if (HttpContext.Session.GetString("UserCart") == null)
            {
                UserCart = new Cart();
                HttpContext.Session.SetString("UserCart", JsonConvert.SerializeObject(UserCart));
            }
            else
            {
                // L?y gi? h�ng t? session
                string cartJson = HttpContext.Session.GetString("UserCart");
                UserCart = JsonConvert.DeserializeObject<Cart>(cartJson);
            }
        }

        public IActionResult OnPostAddKoiFish(long koiFishId)
        {
            // L?y c� Koi t? DAO
            var koiFish = KoiFishDAO.GetKoiFishById(koiFishId);
            if (koiFish != null)
            {
                // Th�m c� Koi v�o gi? h�ng
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
                // Th�m s?n ph?m v�o gi? h�ng
                UserCart.AddProduct(product);
                UpdateCartInSession();
            }

            return RedirectToPage();
        }

        private void UpdateCartInSession()
        {
            // C?p nh?t gi? h�ng v�o session
            HttpContext.Session.SetString("UserCart", JsonConvert.SerializeObject(UserCart));
        }
    }
}
