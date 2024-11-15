using BusinessObject;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Repository.IRepository;
using Repository.Repository;

namespace KoiFarmRazorPage.Pages.Customer
{
    public class KoiFishDetailModel : PageModel
    {
        private readonly IKoiFishRepository koiFishRepository;
        private readonly ICartRepository _cartRepository;

        public KoiFishDetailModel(ICartRepository cartRepository)
        {
            koiFishRepository = new KoiFishRepository();
            _cartRepository = cartRepository;
        }

        [BindProperty] 
        public string? Message { get; set; }
        public KoiFish KoiFish { get; set; }
        public bool IsInCart { get; set; }

        public async Task<IActionResult> OnGetAsync(long id, string message)
        {
            // Lấy thông tin cá koi theo ID
            KoiFish = await koiFishRepository.GetKoiFishById(id);

            // Nếu cá koi không tồn tại, trả về NotFound
            if (KoiFish == null)
            {
                return NotFound();
            }
            
            if (User.Identity.IsAuthenticated)
            {
                var cartItems = _cartRepository.GetCartItems();
                IsInCart = cartItems.Any(item => item.Id == id && item.ItemType == "KoiFish");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCart(long KoiFishId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Auth/Login");
            }

            // Check if the Koi fish is already in the cart
            var cartItems = _cartRepository.GetCartItems();
            if (cartItems.Any(item => item.Id == KoiFishId && item.ItemType == "KoiFish"))
            {
                TempData["Message"] = "Cá Koi này đã ở trong giỏ hàng của bạn rồi!";
                return RedirectToPage("/Customer/KoiFishDetail", new { id = KoiFishId });
            }

            KoiFish koiFish = await koiFishRepository.GetKoiFishById(KoiFishId);

            if (koiFish != null)
            {
                _cartRepository.AddKoiFish(koiFish, 1);
                TempData["Message"] = "Thêm vào giỏ hàng thành công!";
            }
            else
            {
                TempData["Message"] = "Không thể thêm cá Koi vào giỏ hàng, hãy thử lại sau.";
            }

            return RedirectToPage("/Customer/KoiFishDetail", new { id = KoiFishId });
        }
    }
}