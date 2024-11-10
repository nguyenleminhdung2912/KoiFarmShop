using BusinessObject;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Customer
{
    public class CartPageModel : PageModel
    {
        private readonly ICartRepository _cartRepository;

        public CartModel(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public Cart Cart { get; set; }

        public void OnGet()
        {
            Cart = _cartRepository.GetCart();
        }

        public IActionResult OnPostUpdateQuantity(long id, string itemType, int quantity)
        {
            _cartRepository.UpdateQuantity(id, itemType, quantity);
            return RedirectToPage();
        }

        public IActionResult OnPostRemoveItem(long id, string itemType)
        {
            _cartRepository.RemoveItem(id, itemType);
            return RedirectToPage();
        }
    }
}
