using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Customer
{
    [Authorize] 
    public class CartPageModel : PageModel
    {
        private readonly ICartRepository _cartRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
    
        public CartPageModel(
            ICartRepository cartRepository,
            IWalletRepository walletRepository,
            IOrderRepository orderRepository,
            IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _walletRepository = walletRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }
        public Cart Cart { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        

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
        
        
        public async Task<IActionResult> OnPostCheckout(string shippingAddress)
        {
            // Get current user
            var userEmail = User.Identity.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Account/Login");
            }

            // Get cart
            Cart = _cartRepository.GetCart();
            if (Cart.TotalQuantity == 0)
            {
                ErrorMessage = "Your cart is empty";
                return Page();
            }

            // Check wallet balance
            var wallet = await _walletRepository.GetWalletByUserEmail(userEmail);
            if (wallet == null)
            {
                ErrorMessage = "Wallet not found";
                return Page();
            }

            if (wallet.Total < Cart.TotalPrice)
            {
                ErrorMessage = $"Insufficient funds. Please add {(Cart.TotalPrice - wallet.Total):C} to your wallet";
                return Page();
            }

            try
            {
                // Create order
                var order = new Order
                {
                    UserId = wallet.UserId,
                    KoiFishId = string.Join(",", Cart.KoiFishItems.Select(k => k.Id)),
                    ProductId = string.Join(",", Cart.ProductItems.Select(p => p.Id)),
                    Quantity = Cart.TotalQuantity,
                    TotalPrice = Cart.TotalPrice,
                    Status = "PAID",
                    ShipmentStatus = "NOTYET",
                    Address = shippingAddress,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    IsDeleted = false
                };

                // Process payment
                wallet.Total -= Cart.TotalPrice;
                wallet.UpdateAt = DateTime.Now;
                
                // Save changes
                _walletRepository.Update(wallet);
                _orderRepository.SaveOrder(order);
                
                // Clear cart
                _cartRepository.ClearCart();

                SuccessMessage = "Order placed successfully!";
                return RedirectToPage("/Customer/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while processing your order. Please try again.";
                // Log the exception
                return Page();
            }
        }
    }
}
