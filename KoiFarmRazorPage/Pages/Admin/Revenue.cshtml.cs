using BusinessObject;
using BusinessObject.DTO;
using DataAccessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using Repository.Repository;

namespace KoiFarmRazorPage.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class RevenueModel : PageModel
    {

        private readonly IOrderRepository orderRepository;
        private readonly IAdminRepository adminRepository;

        public RevenueModel()
        {
            orderRepository = new OrderRepository();
            adminRepository = new AdminRepository();
        }
        // Tổng doanh thu
        public double? TodayRevenue { get; set; } = default!;
        public double? MonthRevenue { get; set; } = default!;
        public double? YearRevenue { get; set; } = default!;

        // Dữ liệu doanh thu theo các mốc thời gian
        public ChartData DailyRevenueData { get; set; } = new ChartData();
        public ChartData MonthlyRevenueData { get; set; } = new ChartData();
        public ChartData YearlyRevenueData { get; set; } = new ChartData();

        public async Task OnGet()
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
                    "Sunday", "Monday", "Tuesday", "Wednesday",
                    "Thursday", "Friday", "Saturday"
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

