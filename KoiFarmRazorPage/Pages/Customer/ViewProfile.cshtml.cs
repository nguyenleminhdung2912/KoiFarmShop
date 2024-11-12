﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using DataAccessObject;
using KoiFarmRazorPage.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;
using Repository.Repository;

namespace KoiFarmRazorPage.Pages.Customer
{
    [Authorize(Roles = "Customer")]
    public class ViewProfileModel : PageModel
    {
        private readonly IUserRepository userRepository;
        private readonly IWalletRepository walletRepository;
        private readonly IWalletLogRepository walletLogRepository;
        private readonly IVnPayService _vnPayService;
        private readonly IOrderRepository orderRepository;
        private readonly IKoiFishRepository koiFishRepository;
        private readonly IProductRepository productRepository;
        private readonly IHubContext<SignalRHub> hubContext;

        public ViewProfileModel(IVnPayService vnPayService, IHubContext<SignalRHub> hubContext)
        {
            userRepository = new UserRepository();
            walletRepository = new WalletRepository();
            walletLogRepository =  new WalletLogRepository();
            _vnPayService = vnPayService;
            orderRepository = new OrderRepository();
            koiFishRepository = new KoiFishRepository();
            productRepository = new ProductRepository();
            this.hubContext = hubContext;
        }

        [BindProperty] public User UserProfile { get; set; }

        public Wallet UserWallet { get; set; } = default!;
        public List<WalletLog> WalletLogs { get; set; } = default!;
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }
        public List<Order> Orders { get; set; } = default!;
        

        public async Task<IActionResult> OnGet(int pageIndex = 1)
        {
            // Lấy Email lưu từ lúc login
            var Email = User.Identity.Name;
            
            // Lấy người dùng đang đăng nhập
            UserProfile = userRepository.GetUserByEmail(Email);
            
            // Ví người dùng
            UserWallet = await walletRepository.GetWalletByUserEmail(Email);
            
            var walletLogInfo = await walletLogRepository.GetWalletLogsByWalletId(pageIndex, 10, UserWallet.WalletId);
            WalletLogs = walletLogInfo.WalletLogs;
            PageIndex = walletLogInfo.PageIndex;
            TotalPages = walletLogInfo.TotalPages;
            
            Orders = orderRepository.GetOrdersByAccount(UserProfile.UserId);
            foreach (var order in Orders)
            {
                List<Product> products = productRepository.GetProductsByListString(order.ProductId);
                List<KoiFish> koiFishs = koiFishRepository.GetKoiFishsByListString(order.KoiFishId);
                order.KoiFishList = koiFishs;
                order.ProductList = products;
            }
            return Page();
        }

        // Update profile
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Update thông tin profile
                UserProfile.IsDeleted = false;
                userRepository.UpdateUser(UserProfile);
                
                // Tạo thông báo
                TempData["SuccessMessage"] = "Cập nhật thành công!";
                await hubContext.Clients.All.SendAsync("RefreshData");

                return RedirectToPage(); 
            }

            return Page();
        }

        
        // Thanh toán VNPay
        public IActionResult OnPostPayment(long amount)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError(string.Empty, "Số tiền không hợp lệ.");
                return Page();
            }

            // Tạo URL thanh toán và chuyển hướng tới VNPay
            
            hubContext.Clients.All.SendAsync("RefreshData");

            return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, amount));
        }
        
        // Huỷ order
        public async Task<IActionResult> OnPostCancelOrderAsync(int orderId)
        {
            var userEmail = User.Identity?.Name;
            var user = new User();
            if (userEmail != null)
            {
                user = userRepository.GetUserByEmail(userEmail);
            }

            var order = await orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }
            
            
            // Huỷ đơn hàng
            await orderRepository.CancelOrder(order, user.UserId);
            
            TempData["SuccessMessage"] = "Order has been successfully cancelled.";
            
            await hubContext.Clients.All.SendAsync("RefreshData");
            
            return RedirectToPage(); // Quay lại trang sau khi hủy
        }
    }
}