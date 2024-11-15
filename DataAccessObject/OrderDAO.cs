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
            catch (Exception ex)
            {
            }

            return list;
        }

        public static async Task<Order> GetOrderById(long? id)
        {
            using var db = new KoiFarmShopDatabaseContext();
            Order returnOrder
                = await db.Orders
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.OrderId == id);
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
            catch (Exception ex)
            {
            }

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
            catch (Exception ex)
            {
            }

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

        public static async Task<bool> UpdateOrder(Order order)
        {
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                context.Orders.Update(order);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<bool> CancelOrder(Order order, long userId, List<KoiFish> koiFishes, List<Product> products)
        {
            try
            {
                using var context = new KoiFarmShopDatabaseContext();

                var wallet = await context.Wallets
                    .FirstOrDefaultAsync(w => w.UserId == userId);
                if (wallet == null)
                {
                    return false;
                }

                // Kiểm tra TotalPrice và wallet.Total để tránh lỗi null
                if (order.TotalPrice == null || wallet.Total == null)
                {
                    throw new InvalidOperationException("Order total price or wallet balance is null.");
                }

                // Tính tiền phải hoàn
                double refundAmount = 0;

                // Trạng thái "NOTYET" -> hoàn 100%
                if (order.ShipmentStatus.Equals("NOTYET"))
                {
                    refundAmount = order.TotalPrice.Value; // hoàn 100%
                }
                // Trạng thái "PREPARING" -> hoàn 90%
                else if (order.ShipmentStatus.Equals("PREPARING"))
                {
                    refundAmount = order.TotalPrice.Value * 0.9; // hoàn 90%
                }
                // Thêm xử lý nếu trạng thái không hợp lệ
                else
                {
                    throw new InvalidOperationException("Invalid shipment status for cancellation.");
                }

                // Cập nhật số tiền trong ví
                wallet.Total += refundAmount;
                order.Status = "CANCELLED";
                order.ShipmentStatus = "CANCELLED";

                foreach (var product in products)
                {
                    product.Quantity = 1;
                    product.Status = "Available";
                    context.Products.Update(product);
                }

                foreach (var koiFish in koiFishes)
                {
                    koiFish.Quantity = 1;
                    koiFish.Status = "Available";
                    context.KoiFishes.Update(koiFish);
                }

                // Tạo wallet log
                var maxId = context.WalletLogs.Max(a => (int?)a.WalletLogId) ?? 0;
                WalletLog walletLog = new WalletLog();
                walletLog.WalletLogId = (short)(maxId + 1);
                walletLog.WalletId = wallet.WalletId;
                walletLog.Amount = refundAmount;
                walletLog.Type = "Refund";
                walletLog.CreateAt = DateTime.Now;
                walletLog.IsDeleted = false;

                // Lưu thay đổi vào cơ sở dữ liệu
                context.WalletLogs.Add(walletLog);
                context.Orders.Update(order);
                context.Wallets.Update(wallet);
                await context.SaveChangesAsync();
                return true;
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
                    .Where(o => o.CreateAt.HasValue && o.CreateAt.Value.Year >= startYear &&
                                o.Status.Equals("Completed"))
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

        public static List<Order> GetOrdersByShip(string ship)
        {
            using var context = new KoiFarmShopDatabaseContext();
            if (ship == "ALL")
            {
                return context.Orders.Include(o => o.User).ToList();
            }
            else
            {
                return context.Orders.Include(o => o.User).Where(o => o.ShipmentStatus.Equals(ship)).ToList();
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

        public static bool SetShipStatusOrder(long orderId, string shipStatus)
        {
            using var _context = new KoiFarmShopDatabaseContext();
            var existingOrder = _context.Orders.Find(orderId);
            if (existingOrder != null)
            {
                try
                {
                    existingOrder.ShipmentStatus = shipStatus;
                    if (shipStatus.Equals("SUCCESSFUL"))
                    {
                        existingOrder.Status = "COMPLETED";
                    }
                    _context.Orders.Update(existingOrder);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return false;
        }

        public static void UpdateOrderByCancel(Order order)
        {
            using var context = new KoiFarmShopDatabaseContext();
            var existingOrder = context.Orders.Find(order.OrderId);
            if (existingOrder != null)
            {
                existingOrder.Status = order.Status;
                existingOrder.ShipmentStatus = order.ShipmentStatus;
                context.Orders.Update(existingOrder);
                context.SaveChanges();
            }
        }
        public static Order GetOrderByIdNotAsync(long orderId)
        {
            using var _context = new KoiFarmShopDatabaseContext();
            return _context.Orders.Include(o => o.User).FirstOrDefault(o => o.OrderId == orderId);
        }
    }
}