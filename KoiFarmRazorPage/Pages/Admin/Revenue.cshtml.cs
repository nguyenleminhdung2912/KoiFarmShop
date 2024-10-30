using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KoiFarmRazorPage.Pages.Admin
{
    public class RevenueModel : PageModel
    {
        // Tổng doanh thu
        public decimal TodayRevenue { get; set; }
        public decimal MonthRevenue { get; set; }
        public decimal YearRevenue { get; set; }

        // Dữ liệu doanh thu theo các mốc thời gian
        public ChartData DailyRevenueData { get; set; }
        public ChartData MonthlyRevenueData { get; set; }
        public ChartData YearlyRevenueData { get; set; }

        public void OnGet()
        {
            // Gán dữ liệu mẫu cho tổng doanh thu
            TodayRevenue = 2000000;    // Ví dụ: 2 triệu VNĐ
            MonthRevenue = 50000000;   // Ví dụ: 50 triệu VNĐ
            YearRevenue = 600000000;   // Ví dụ: 600 triệu VNĐ

            // Gán dữ liệu mẫu cho biểu đồ doanh thu
            DailyRevenueData = new ChartData
            {
                Labels = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" },
                Values = new List<decimal> { 500000, 700000, 600000, 800000, 750000, 850000, 500000 }
            };

            MonthlyRevenueData = new ChartData
            {
                Labels = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },
                Values = new List<decimal> { 30000000, 25000000, 40000000, 45000000, 50000000, 55000000, 60000000, 65000000, 70000000, 75000000, 80000000, 85000000 }
            };

            YearlyRevenueData = new ChartData
            {
                Labels = new List<string> { "2019", "2020", "2021", "2022", "2023" },
                Values = new List<decimal> { 300000000, 350000000, 400000000, 450000000, 500000000 }
            };
        }

        // Lớp giúp truyền dữ liệu vào biểu đồ
        public class ChartData
        {
            public List<string> Labels { get; set; } = new List<string>();
            public List<decimal> Values { get; set; } = new List<decimal>();
        }
    }
}

