using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderById(long? id);

        List<Order> GetAllOrders();

        List<Order> GetOrdersByAccount(long accountId);

        List<Order> GetOrdersByFromDateToDate(DateTime fromDate, DateTime toDate);
        Task<bool> UpdateOrder(Order order);
        Task<bool> CancelOrder(Order order, long userId);
        void DeleteOrder(Order order);
        void SaveOrder(Order order);
        
        List<Order> GetOrdersByShipStatus(string shipStatus);
        
        bool SetShipStatusOrder(long orderId, string shipStatus);
    }
}
