using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff;
[Authorize(Roles = "Staff")]

public class ShipManagement : PageModel
{
    public string SelectedStatus { get; set; }

    private readonly IOrderRepository _orderRepository;

    private readonly IProductRepository _productRepository;

    private readonly IKoiFishRepository _koiFishRepository;
    private readonly IHubContext<SignalRHub> hubContext;


    public ShipManagement(IOrderRepository orderRepository, IProductRepository productRepository,
        IKoiFishRepository koiFishRepository, IHubContext<SignalRHub> hubContext)
    {
        this._orderRepository = orderRepository;
        this._productRepository = productRepository;
        this._koiFishRepository = koiFishRepository;
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
            SelectedStatus = "NOTYET";
            if (string.IsNullOrEmpty(Request.Form["selectedOrderId"]))
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
                TempData["Fail"] = "Please select specific order to prepare ";
            }
            else
            {
                SelectedStatus = "PREPARING";
                if (_orderRepository.SetShipStatusOrder(long.Parse(Request.Form["selectedOrderId"]), SelectedStatus))
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
                    hubContext.Clients.All.SendAsync("RefreshData");

                    TempData["Success"] = "Ship prepared status updated sucessfully";
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

                    TempData["Fail"] = "Ship prepared status updated failed";
                }
            }
        }
        
        if (handler == "OnGoing")
        {
            SelectedStatus = "PREPARING";
            if (string.IsNullOrEmpty(Request.Form["selectedOrderId"]))
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
                TempData["Fail"] = "Please select specific order to set ship status on going ";
            }
            else
            {
                SelectedStatus = "ONGOING";
                if (_orderRepository.SetShipStatusOrder(long.Parse(Request.Form["selectedOrderId"]), SelectedStatus))
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
                    hubContext.Clients.All.SendAsync("RefreshData");

                    TempData["Success"] = "Ship on going status updated sucessfully";
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

                    TempData["Fail"] = "Ship on going status updated failed";
                }
            }
        }
        
        
         if (handler == "Success")
        {
            SelectedStatus = "ONGOING";
            if (string.IsNullOrEmpty(Request.Form["selectedOrderId"]))
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
                TempData["Fail"] = "Please select specific order to set ship status success";
            }
            else
            {
                SelectedStatus = "SUCCESSFUL";
                if (_orderRepository.SetShipStatusOrder(long.Parse(Request.Form["selectedOrderId"]), SelectedStatus))
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
                    hubContext.Clients.All.SendAsync("RefreshData");

                    TempData["Success"] = "Ship sucesss status updated sucessfully";
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

                    TempData["Fail"] = "Ship sucess status updated failed";
                }
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