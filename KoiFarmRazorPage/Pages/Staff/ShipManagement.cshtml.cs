using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;
using Repository.Repository;

namespace KoiFarmRazorPage.Pages.Staff;

[Authorize(Roles = "Staff")]
public class ShipManagement : PageModel
{
    public string SelectedStatus { get; set; }

    private readonly IOrderRepository _orderRepository;

    private readonly IProductRepository _productRepository;

    private readonly IKoiFishRepository _koiFishRepository;

    private readonly IUserRepository _userRepository;

    private readonly IWalletRepository _walletRepository;

    private readonly IHubContext<SignalRHub> hubContext;


    public ShipManagement(IOrderRepository orderRepository, IProductRepository productRepository,
        IKoiFishRepository koiFishRepository, IHubContext<SignalRHub> hubContext, IUserRepository userRepository,
        IWalletRepository walletRepository)
    {
        this._orderRepository = orderRepository;
        this._productRepository = productRepository;
        this._koiFishRepository = koiFishRepository;
        this._userRepository = userRepository;
        this._walletRepository = walletRepository;
        this.hubContext = hubContext;
    }

    public List<Order> Orders { get; set; } = new List<Order>();

    // public Dictionary<long, List<long>> KoiFishIdsByOrder { get; set; } = new Dictionary<long, List<long>>();
    //
    // public Dictionary<long, List<long>> ProductIdsByOrder { get; set; } = new Dictionary<long, List<long>>();

    public void OnGet()
    {
        Orders = _orderRepository.GetAllOrders();
        foreach (var order in Orders)
        {
            // KoiFishIdsByOrder[order.OrderId] = ParseIdString(order.KoiFishId);
            //
            // ProductIdsByOrder[order.OrderId] = ParseIdString(order.ProductId);
            var koiFishIdList = ParseIdString(order.KoiFishId);
            var productIdList = ParseIdString(order.ProductId);
            foreach (var koiFishId in koiFishIdList)
            {
                if (_koiFishRepository.GetKoiFishByIdByStaff(koiFishId) != null)
                {
                    order.KoiFishList.Add(_koiFishRepository.GetKoiFishByIdByStaff(koiFishId));
                }
            }

            foreach (var productId in koiFishIdList)
            {
                if (_productRepository.GetProductById(productId) != null)
                {
                    order.ProductList.Add(_productRepository.GetProductById(productId));
                }
            }
        }
    }

    public void OnPost()
    {
        SelectedStatus = Request.Form["shipStatus"];
        string handler = Request.Form["handler"];
        if (handler == "Search")
        {
            Orders = _orderRepository.GetOrdersByShipStatus(SelectedStatus);
            foreach (var order in Orders)
            {
                // KoiFishIdsByOrder[order.OrderId] = ParseIdString(order.KoiFishId);
                //
                // ProductIdsByOrder[order.OrderId] = ParseIdString(order.ProductId);
                var koiFishIdList = ParseIdString(order.KoiFishId);
                var productIdList = ParseIdString(order.ProductId);
                foreach (var koiFishId in koiFishIdList)
                {
                    if (_koiFishRepository.GetKoiFishByIdByStaff(koiFishId) != null)
                    {
                        order.KoiFishList.Add(_koiFishRepository.GetKoiFishByIdByStaff(koiFishId));
                    }
                }

                foreach (var productId in koiFishIdList)
                {
                    if (_productRepository.GetProductById(productId) != null)
                    {
                        order.ProductList.Add(_productRepository.GetProductById(productId));
                    }
                }
            }
        }


        if (handler == "Prepare")
        {
            // SelectedStatus = "NOTYET";
            SelectedStatus = "PREPARING";
            if (_orderRepository.SetShipStatusOrder(long.Parse(Request.Form["orderId"]), SelectedStatus))
            {
                Orders = _orderRepository.GetAllOrders();
                foreach (var order in Orders)
                {
                    // KoiFishIdsByOrder[order.OrderId] = ParseIdString(order.KoiFishId);
                    //
                    // ProductIdsByOrder[order.OrderId] = ParseIdString(order.ProductId);
                    var koiFishIdList = ParseIdString(order.KoiFishId);
                    var productIdList = ParseIdString(order.ProductId);
                    foreach (var koiFishId in koiFishIdList)
                    {
                        if (_koiFishRepository.GetKoiFishByIdByStaff(koiFishId) != null)
                        {
                            order.KoiFishList.Add(_koiFishRepository.GetKoiFishByIdByStaff(koiFishId));
                        }
                    }

                    foreach (var productId in koiFishIdList)
                    {
                        if (_productRepository.GetProductById(productId) != null)
                        {
                            order.ProductList.Add(_productRepository.GetProductById(productId));
                        }
                    }
                }


                TempData["Success"] = "Cập nhật trạng thái chuẩn bị đơn hàng thành công ";
            }
            else
            {
                Orders = _orderRepository.GetAllOrders();
                foreach (var order in Orders)
                {
                    // KoiFishIdsByOrder[order.OrderId] = ParseIdString(order.KoiFishId);
                    //
                    // ProductIdsByOrder[order.OrderId] = ParseIdString(order.ProductId);
                    var koiFishIdList = ParseIdString(order.KoiFishId);
                    var productIdList = ParseIdString(order.ProductId);
                    foreach (var koiFishId in koiFishIdList)
                    {
                        if (_koiFishRepository.GetKoiFishByIdByStaff(koiFishId) != null)
                        {
                            order.KoiFishList.Add(_koiFishRepository.GetKoiFishByIdByStaff(koiFishId));
                        }
                    }

                    foreach (var productId in koiFishIdList)
                    {
                        if (_productRepository.GetProductById(productId) != null)
                        {
                            order.ProductList.Add(_productRepository.GetProductById(productId));
                        }
                    }
                }

                TempData["Fail"] = "Cập nhật trạng thái chuẩn bị đơn hàng không thành công";
            }
        }

        if (handler == "OnGoing")
        {
            string orderId = Request.Form["orderId"];
            SelectedStatus = "ONGOING";
            if (_orderRepository.SetShipStatusOrder(long.Parse(Request.Form["orderId"]), SelectedStatus))
            {
                Orders = _orderRepository.GetAllOrders();
                foreach (var order in Orders)
                {
                    // KoiFishIdsByOrder[order.OrderId] = ParseIdString(order.KoiFishId);
                    //
                    // ProductIdsByOrder[order.OrderId] = ParseIdString(order.ProductId);
                    var koiFishIdList = ParseIdString(order.KoiFishId);
                    var productIdList = ParseIdString(order.ProductId);
                    foreach (var koiFishId in koiFishIdList)
                    {
                        if (_koiFishRepository.GetKoiFishByIdByStaff(koiFishId) != null)
                        {
                            order.KoiFishList.Add(_koiFishRepository.GetKoiFishByIdByStaff(koiFishId));
                        }
                    }

                    foreach (var productId in koiFishIdList)
                    {
                        if (_productRepository.GetProductById(productId) != null)
                        {
                            order.ProductList.Add(_productRepository.GetProductById(productId));
                        }
                    }
                }

                TempData["Success"] = "Cập nhật trạng thái đang giao đơn hàng thành công";
            }
            else
            {
                Orders = _orderRepository.GetAllOrders();
                foreach (var order in Orders)
                {
                    // KoiFishIdsByOrder[order.OrderId] = ParseIdString(order.KoiFishId);
                    //
                    // ProductIdsByOrder[order.OrderId] = ParseIdString(order.ProductId);
                    var koiFishIdList = ParseIdString(order.KoiFishId);
                    var productIdList = ParseIdString(order.ProductId);
                    foreach (var koiFishId in koiFishIdList)
                    {
                        if (_koiFishRepository.GetKoiFishByIdByStaff(koiFishId) != null)
                        {
                            order.KoiFishList.Add(_koiFishRepository.GetKoiFishByIdByStaff(koiFishId));
                        }
                    }

                    foreach (var productId in koiFishIdList)
                    {
                        if (_productRepository.GetProductById(productId) != null)
                        {
                            order.ProductList.Add(_productRepository.GetProductById(productId));
                        }
                    }
                }

                TempData["Fail"] = "Cập nhật trạng thái đang giao đơn hàng thất bại";
            }
        }

        if (handler == "Cancel")
        {
            Order order = _orderRepository.GetOrderByIdNotAsync(long.Parse(Request.Form["orderId"]));
            if (order.Status == "SUCCESSFUL")
            {
                TempData["Fail"] = "Không thể huỷ đơn hàng vì đơn hàng đã hoàn tất!!!";
            }
            else if (order.Status == "CANCELLED")
            {
                TempData["Fail"] = "Đơn hàng này đã huỷ rồi!!!";
            }
            else if (order.Status == "ONGOING")
            {
                TempData["Fail"] = "Không thể huỷ đơn hàng vì đơn hàng đang giao!!!";
            }
            else
            {
                User user = _userRepository.GetUserById(long.Parse(User.FindFirst("userId").Value));
                if (order.Status == "PAID")
                {
                    foreach (var wallet in user.Wallets)
                    {
                        wallet.Total += order.TotalPrice.GetValueOrDefault();
                        _walletRepository.Refund(wallet);
                    }

                    order.Status = "CANCELLED";
                    order.ShipmentStatus = "CANCELLED";
                    _orderRepository.UpdateOrderByCancel(order);
                }
                else
                {
                    order.Status = "CANCELLED";
                    order.ShipmentStatus = "CANCELLED";
                    _orderRepository.UpdateOrderByCancel(order);
                }
            }
        }

        if (handler == "Success")
        {
            SelectedStatus = "SUCCESSFUL";
            if (_orderRepository.SetShipStatusOrder(long.Parse(Request.Form["orderId"]), SelectedStatus))
            {
                Orders = _orderRepository.GetAllOrders();
                foreach (var order in Orders)
                {
                    // KoiFishIdsByOrder[order.OrderId] = ParseIdString(order.KoiFishId);
                    //
                    // ProductIdsByOrder[order.OrderId] = ParseIdString(order.ProductId);
                    var koiFishIdList = ParseIdString(order.KoiFishId);
                    var productIdList = ParseIdString(order.ProductId);
                    foreach (var koiFishId in koiFishIdList)
                    {
                        if (_koiFishRepository.GetKoiFishByIdByStaff(koiFishId) != null)
                        {
                            order.KoiFishList.Add(_koiFishRepository.GetKoiFishByIdByStaff(koiFishId));
                        }
                    }

                    foreach (var productId in koiFishIdList)
                    {
                        if (_productRepository.GetProductById(productId) != null)
                        {
                            order.ProductList.Add(_productRepository.GetProductById(productId));
                        }
                    }
                }

                TempData["Success"] = "Cập nhật trạng thái đã giao thành công đơn hàng thành công";
            }
            else
            {
                Orders = _orderRepository.GetAllOrders();
                foreach (var order in Orders)
                {
                    // KoiFishIdsByOrder[order.OrderId] = ParseIdString(order.KoiFishId);
                    //
                    // ProductIdsByOrder[order.OrderId] = ParseIdString(order.ProductId);
                    var koiFishIdList = ParseIdString(order.KoiFishId);
                    var productIdList = ParseIdString(order.ProductId);
                    foreach (var koiFishId in koiFishIdList)
                    {
                        if (_koiFishRepository.GetKoiFishByIdByStaff(koiFishId) != null)
                        {
                            order.KoiFishList.Add(_koiFishRepository.GetKoiFishByIdByStaff(koiFishId));
                        }
                    }

                    foreach (var productId in koiFishIdList)
                    {
                        if (_productRepository.GetProductById(productId) != null)
                        {
                            order.ProductList.Add(_productRepository.GetProductById(productId));
                        }
                    }
                }

                TempData["Fail"] = "Cập nhật trạng thái đã giao thành công đơn hàng thất bại";
            }
        }
    }

    private List<long> ParseIdString(string? idString)
    {
        List<long> resultList = new List<long>();
        if (string.IsNullOrWhiteSpace(idString))
        {
            return resultList;
        }

        string[] idArray = idString.Split(',');
        foreach (var id in idArray)
        {
            long parsedId;
            // Thử chuyển đổi chuỗi thành số nguyên
            if (long.TryParse(id, out parsedId) && parsedId > 0)
            {
                resultList.Add(parsedId); // Chỉ thêm vào danh sách nếu là số nguyên dương
            }
        }

        return resultList;
    }
}