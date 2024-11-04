using BusinessObject;
using BusinessObject.DTO;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject
{
    public class OrderDAO
    {
        public static void DeleteOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public static List<Order> GetAllOrders()
        {
            var list = new List<Order>();
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                list = context.Orders
                    .Include(o => o.User)
                    .ToList();
            }
            catch (Exception ex) { }
            return list;
        }

        public static Order GetOrderById(long? id)
        {
            using var db = new KoiFarmShopDatabaseContext();
            Order returnOrder
                = db.Orders
                .FirstOrDefault(c => c.OrderId.Equals(id));
            return returnOrder;
        }

        public static List<Order> GetOrdersByAccount(long accountId)
        {
            var list = new List<Order>();
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                list = context.Orders
                    .Where(o => o.UserId.Equals(accountId))
                    .ToList();
            }
            catch (Exception ex) { }
            return list;
        }

        public static List<Order> GetOrdersByFromDateToDate(DateTime fromDate, DateTime toDate)
        {
            var list = new List<Order>();
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                list = context.Orders
                    .Where(o => o.CreateAt >= fromDate && o.CreateAt <= toDate)
                    .ToList();
            }
            catch (Exception ex) { }
            return list;
        }

        public static void SaveOrder(Order order)
        {
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                var maxId = context.Orders.Max(a => (int?)a.OrderId) ?? 0;
                order.OrderId = (long)(maxId + 1);
                context.Orders.Add(order);
                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public static void UpdateOrder(Order order)
        {
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                context.Entry<Order>(order).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // ** ADMIN FLOW ** //

        // 1. Lấy danh sách Orders từ ngày A tới ngày B
        public static async Task<List<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                using var _context = new KoiFarmShopDatabaseContext();
                return await _context.Orders
                    .Where(o => o.CreateAt >= startDate && o.CreateAt <= endDate && o.Status.Equals("Completed"))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Order>();
            }
        }

        // 2. Lấy danh sách Orders trong tháng này
        public static async Task<List<Order>> GetOrdersThisMonthAsync()
        {
            try
            {
                using var _context = new KoiFarmShopDatabaseContext();
                DateTime now = DateTime.Now;
                DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                return await _context.Orders
                    .Where(o => o.CreateAt >= startOfMonth && o.CreateAt <= endOfMonth && o.Status.Equals("Completed"))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Order>();
            }
        }

        // 3. Lấy danh sách Orders trong tuần này
        public static async Task<List<Order>> GetOrdersThisWeekAsync()
        {
            try
            {
                using var _context = new KoiFarmShopDatabaseContext();
                DateTime startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
                DateTime endOfWeek = startOfWeek.AddDays(7).AddTicks(-1);

                return await _context.Orders
                    .Where(o => o.CreateAt >= startOfWeek && o.CreateAt <= endOfWeek && o.Status.Equals("Completed"))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Order>();
            }
        }

        // 4. Lấy danh sách Orders trong năm nay
        public static async Task<List<Order>> GetOrdersThisYearAsync()
        {
            try
            {
                using var _context = new KoiFarmShopDatabaseContext();
                DateTime now = DateTime.Now;
                DateTime startOfYear = new DateTime(now.Year, 1, 1);
                DateTime endOfYear = startOfYear.AddYears(1).AddTicks(-1);

                return await _context.Orders
                    .Where(o => o.CreateAt >= startOfYear && o.CreateAt <= endOfYear && o.Status.Equals("Completed"))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Order>();
            }
        }

        // 5. Lấy danh sách Orders trong 4 năm gần nhất
        public static async Task<List<Order>> GetOrdersNearest4Years()
        {
            try
            {
                var now = DateTime.Now;
                var startYear = now.Year - 3; // Lấy dữ liệu từ 4 năm gần nhất, bao gồm năm hiện tại

                using var _context = new KoiFarmShopDatabaseContext();

                return await _context.Orders
                    .Where(o => o.CreateAt.HasValue && o.CreateAt.Value.Year >= startYear && o.Status.Equals("Completed"))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Order>();
            }
        }

        // 6. Lấy danh sách Orders trong hôm nay
        public static async Task<List<Order>> GetOrdersToday()
        {
            try
            {
                DateTime today = DateTime.Today;
                DateTime tomorrow = today.AddDays(1);

                using var _context = new KoiFarmShopDatabaseContext();

                return await _context.Orders
                    .Where(o => o.CreateAt >= today && o.CreateAt < tomorrow && o.Status.Equals("Completed"))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log exception or handle as needed
                Console.WriteLine(ex.Message);
                return new List<Order>(); // Return an empty list in case of error
            }
        }

        // 7. Lấy tổng doanh thu cho ngày / tháng / năm
        public static async Task<RevenueDTO> GetRevenueDataAsync()
        {
            using var _context = new KoiFarmShopDatabaseContext();
            DateTime today = DateTime.Today;
            DateTime now = DateTime.Now;
            DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);
            DateTime startOfYear = new DateTime(now.Year, 1, 1);

            var revenueData = await _context.Orders
                .Where(o => o.CreateAt.HasValue && o.Status.Equals("Completed"))
                .GroupBy(o => new
                {
                    Today = o.CreateAt.Value.Date,
                    Month = new DateTime(o.CreateAt.Value.Year, o.CreateAt.Value.Month, 1),
                    Year = o.CreateAt.Value.Year
                })
                .Select(g => new
                {
                    TodayRevenue = g.Key.Today == today ? g.Sum(o => o.TotalPrice) : 0,
                    MonthRevenue = g.Key.Month == startOfMonth ? g.Sum(o => o.TotalPrice) : 0,
                    YearRevenue = g.Key.Year == now.Year ? g.Sum(o => o.TotalPrice) : 0
                })
            .ToListAsync();

            return new RevenueDTO
            {
                TodayRevenue = revenueData.Sum(r => r.TodayRevenue),
                MonthRevenue = revenueData.Sum(r => r.MonthRevenue),
                YearRevenue = revenueData.Sum(r => r.YearRevenue)
            };
        }
    }
}
