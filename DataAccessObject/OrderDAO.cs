using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
