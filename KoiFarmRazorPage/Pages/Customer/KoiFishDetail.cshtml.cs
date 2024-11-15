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

        [BindProperty] public string? Message { get; set; }
        public KoiFish KoiFish { get; set; }

        public async Task<IActionResult> OnGetAsync(long id, string message)
        {
            // Lấy thông tin cá koi theo ID
            KoiFish = await koiFishRepository.GetKoiFishById(id);

            // Nếu cá koi không tồn tại, trả về NotFound
            if (KoiFish == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCart(long KoiFishId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Auth/Login");
            }

            KoiFish koiFish = await koiFishRepository.GetKoiFishById(KoiFishId);

            if (koiFish != null)
            {
                _cartRepository.AddKoiFish(koiFish, 1);
            }

            TempData["Message"] = "Add to Cart Successfully! Please check your cart!";

            return RedirectToPage("/Customer/KoiFishDetail", new { id = KoiFishId });
        }
    }
}