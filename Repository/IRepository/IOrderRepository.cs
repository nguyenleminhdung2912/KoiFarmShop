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
        Order GetOrderById(long? id);

        List<Order> GetAllOrders();

        List<Order> GetOrdersByAccount(long accountId);

        List<Order> GetOrdersByFromDateToDate(DateTime fromDate, DateTime toDate);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);
        void SaveOrder(Order order);
    }
}
