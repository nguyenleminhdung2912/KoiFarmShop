using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;
using Repository.Repository;
using static KoiFarmRazorPage.Pages.Admin.RevenueModel;

namespace KoiFarmRazorPage.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        // Properties to hold data

        private readonly IUserRepository userRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IKoiFishRepository koiFishRepository;
        private readonly IProductRepository productRepository;
        private readonly IAdminRepository adminRepository;
        private readonly IHubContext<SignalRHub> hubContext;

        public IList<User> User { get; set; } = new List<User>()!;
        public IList<Order> Order { get; set; } = new List<Order>()!;

        // Tổng doanh thu
        public double? TodayRevenue { get; set; } = default!;
        public double? MonthRevenue { get; set; } = default!;
        public double? YearRevenue { get; set; } = default!;

        // Dữ liệu doanh thu theo các mốc thời gian
        public ChartData DailyRevenueData { get; set; } = new ChartData();
        public ChartData MonthlyRevenueData { get; set; } = new ChartData();
        public ChartData YearlyRevenueData { get; set; } = new ChartData();

        public IndexModel( IHubContext<SignalRHub> hubContext)
        {
            userRepository = new UserRepository();
            orderRepository = new OrderRepository();
            koiFishRepository = new KoiFishRepository();
            productRepository = new ProductRepository();
            adminRepository = new AdminRepository();
            this.hubContext = hubContext;
        }

        public async Task OnGet()
        {
            // Initial data load (could be empty)
            LoadUsers();
            LoadBookings();
            await LoadRevenueChart();
        }
        public async Task<IActionResult> OnPostReActivateAsync(int UserId)
        {
            if (UserId == null)
            {
                return NotFound();
            }

            var user = userRepository.GetUserById(UserId);
            user.IsDeleted = false;
            userRepository.UpdateUser(user);
            await hubContext.Clients.All.SendAsync("RefreshData");

            return RedirectToPage();
        }

        // ** USER ** //
        private void LoadUsers()
        {
            User = userRepository.GetUsers();
        }

        // ** BOOKING ** //
        private void LoadBookings()
        {
            Order = orderRepository.GetAllOrders();
            foreach (var order in Order)
            {
                List<Product> products = productRepository.GetProductsByListString(order.ProductId);
                List<KoiFish> koiFishs = koiFishRepository.GetKoiFishsByListString(order.KoiFishId);
                order.KoiFishList = koiFishs;
                order.ProductList = products;
            }
        }

        // ** REVENUE ** //
        private async Task LoadRevenueChart()
        {
            RevenueDTO revenueDTO = await adminRepository.GetRevenueDataAsync();
            // Gán dữ liệu mẫu cho tổng doanh thu
            TodayRevenue = revenueDTO.TodayRevenue;
            MonthRevenue = revenueDTO.MonthRevenue;
            YearRevenue = revenueDTO.YearRevenue;

            // Gán dữ liệu mẫu cho biểu đồ doanh thu
            DailyRevenueData = await GetDailyRevenueDataAsync();

            MonthlyRevenueData = await GetMonthlyRevenueDataAsync();

            YearlyRevenueData = await GetYearlyRevenueDataAsync();
        }
        // Lớp giúp truyền dữ liệu vào biểu đồ
        public class ChartData
        {
            public List<string> Labels { get; set; } = new List<string>();
            public List<double> Values { get; set; } = new List<double>();
        }
        public async Task<ChartData> GetMonthlyRevenueDataAsync()
        {
            // Lấy danh sách Orders trong năm hiện tại
            List<Order> orders = await adminRepository.GetOrdersThisYear();

            // Tạo một biến lưu tổng doanh thu cho mỗi tháng
            var monthlyRevenue = new Dictionary<int, double>();

            // Khởi tạo doanh thu cho mỗi tháng bằng 0
            for (int month = 1; month <= 12; month++)
            {
                monthlyRevenue[month] = 0;
            }

            // Tính tổng doanh thu cho mỗi tháng
            foreach (var order in orders)
            {
                if (order.CreateAt.HasValue && order.TotalPrice.HasValue)
                {
                    int month = order.CreateAt.Value.Month;
                    monthlyRevenue[month] += order.TotalPrice.Value;
                }
            }

            // Chuẩn bị dữ liệu cho biểu đồ
            var chartData = new ChartData
            {
                Labels = new List<string>
                {
                    "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                    "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
                },
                Values = new List<double>()
            };

            for (int month = 1; month <= 12; month++)
            {
                chartData.Values.Add(monthlyRevenue[month]);
            }

            return chartData;
        }
        public async Task<ChartData> GetDailyRevenueDataAsync()
        {
            // Lấy danh sách Orders trong tuần hiện tại
            var orders = await adminRepository.GetOrdersThisWeek();

            // Tạo một biến để lưu tổng doanh thu cho mỗi ngày trong tuần
            var dailyRevenue = new Dictionary<DayOfWeek, double>();

            // Khởi tạo giá trị cho mỗi ngày tránh conflict
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                dailyRevenue[day] = 0;
            }

            // Tính tổng doanh thu cho mỗi ngày
            foreach (var order in orders)
            {
                if (order.CreateAt.HasValue && order.TotalPrice.HasValue)
                {
                    DayOfWeek day = order.CreateAt.Value.DayOfWeek;
                    dailyRevenue[day] += order.TotalPrice.Value;
                }
            }

            // Chuẩn bị dữ liệu cho biểu đồ
            var chartData = new ChartData
            {
                Labels = new List<string>
                {
                    "CN", "T2", "T3", "T4",
                    "T5", "T6", "T7"
                },
                Values = new List<double>()
            };

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                chartData.Values.Add(dailyRevenue[day]);
            }

            return chartData;
        }
        public async Task<ChartData> GetYearlyRevenueDataAsync()
        {
            var now = DateTime.Now;
            var startYear = now.Year - 3;

            // Lấy danh sách Orders trong 4 năm gần nhất
            var orders = await adminRepository.GetOrdersNearest4Years();

            // Tạo dictionary để lưu tổng doanh thu mỗi năm
            var yearlyRevenue = new Dictionary<int, double>();

            for (int year = startYear; year <= now.Year; year++)
            {
                yearlyRevenue[year] = 0; // Khởi tạo doanh thu mỗi năm bằng 0
            }

            // Tính tổng doanh thu cho từng năm
            foreach (var order in orders)
            {
                if (order.TotalPrice.HasValue)
                {
                    int orderYear = order.CreateAt.Value.Year;
                    yearlyRevenue[orderYear] += order.TotalPrice.Value;
                }
            }

            // Chuẩn bị dữ liệu cho biểu đồ
            var chartData = new ChartData
            {
                Labels = yearlyRevenue.Keys.Select(y => y.ToString()).ToList(),
                Values = yearlyRevenue.Values.ToList()
            };

            return chartData;
        }
    }
}
