// CartPageModel.cs

using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;
using KoiFarmRazorPage.Service;

namespace KoiFarmRazorPage.Pages.Customer
{
    [Authorize(Roles = "Customer")]
    public class CartPageModel : PageModel
    {
        private readonly ICartRepository _cartRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IKoiFishRepository _koiFishRepository;
        private readonly IProductRepository _productRepository;
        private readonly EmailService _emailService;
        public CartPageModel(
            ICartRepository cartRepository,
            IWalletRepository walletRepository,
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IKoiFishRepository koiFishRepository,
            IProductRepository productRepository,
            EmailService emailService)
        {
            _cartRepository = cartRepository;
            _walletRepository = walletRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _koiFishRepository = koiFishRepository;
            _productRepository = productRepository;
            _emailService = emailService;
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
            // Only allow quantity updates for products, not for koi fish
            if (itemType == "Product")
            {
                _cartRepository.UpdateQuantity(id, itemType, quantity);
            }

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
                ErrorMessage = $"Số tiền trong ví không đủ. Nãy nạp {(Cart.TotalPrice - wallet.Total):N0} VND vào ví để thanh toán.";
                return Page();
            }

            // Check koi fish availability by status
            bool isAvailable = true;
            foreach (var item in Cart.KoiFishItems)
            {
                var koiFish = await _koiFishRepository.GetKoiFishById(item.Id);
                if (koiFish == null || koiFish.Status != "Available")
                {
                    ErrorMessage = $"The koi fish {koiFish?.Name ?? "Unknown"} is no longer available for purchase.";
                    isAvailable = false;
                    break;
                }
            }

            // Check product availability
            foreach (var item in Cart.ProductItems)
            {
                var product = _productRepository.GetProductById(item.Id);
                if (product == null || product.Quantity < item.Quantity)
                {
                    ErrorMessage = $"Not enough {product?.Name} in stock. Only {product?.Quantity ?? 0} available.";
                    isAvailable = false;
                    break;
                }
            }

            if (!isAvailable)
            {
                return Page();
            }

            try
            {
                // Create order
                var koiFishIds = Cart.KoiFishItems.Select(item => item.Id.ToString()).ToList();
                var productIds = Cart.ProductItems.SelectMany(item =>
                    Enumerable.Repeat(item.Id.ToString(), item.Quantity)
                ).ToList();

                var order = new Order
                {
                    UserId = wallet.UserId,
                    KoiFishId = koiFishIds.Any() ? string.Join(",", koiFishIds) : null,
                    ProductId = productIds.Any() ? string.Join(",", productIds) : null,
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

                // Update koi fish status to SOLD
                foreach (var item in Cart.KoiFishItems)
                {
                    _koiFishRepository.UpdateKoiFishStatus(item.Id, "Sold Out");
                }

                // Update product quantities
                foreach (var item in Cart.ProductItems)
                {
                    var product = _productRepository.GetProductById(item.Id);
                    if (product != null)
                    {
                        product.Quantity -= item.Quantity;
                        if (product.Quantity == 0)
                        {
                            product.Status = "Sold Out";
                            product.Quantity = 0;
                        }

                        _productRepository.UpdateProduct(product);
                    }
                }

                // Save changes
                _walletRepository.Update(wallet);
                _orderRepository.SaveOrder(order);

                // Clear cart
                _cartRepository.ClearCart();

                SuccessMessage = "Order placed successfully!";
                var user = _userRepository.GetUserByEmail(userEmail);
                var subject = "Đơn hàng đã được đặt thành công!";
                var body = $@"
            <p>Chào {user.Name},</p>
            <p>Chúc mừng đơn hàng của bạn đã được đặt thành công.</p>
            <p>Chúc bạn có một trải nghiệm tuyệt vời tại KoiFarm!</p>
            <p>Trân trọng,<br/>Đội ngũ Koi Farm Shop</p>
        ";

                await _emailService.SendEmailAsync(user.Email, subject, body);
                return RedirectToPage("/Customer/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while processing your order. Please try again.";
                return Page();
            }
        }
    }
}