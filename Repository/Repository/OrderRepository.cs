using BusinessObject;
using DataAccessObject;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public void DeleteOrder(Order order)
        => OrderDAO.DeleteOrder(order);

        public List<Order> GetAllOrders()
        => OrderDAO.GetAllOrders();

        public Order GetOrderById(long? id)
        => OrderDAO.GetOrderById(id);

        public List<Order> GetOrdersByAccount(long accountId)
        => OrderDAO.GetOrdersByAccount(accountId);

        public List<Order> GetOrdersByFromDateToDate(DateTime fromDate, DateTime toDate)
        => OrderDAO.GetOrdersByFromDateToDate(fromDate, toDate);

        public void SaveOrder(Order order)
        => OrderDAO.SaveOrder(order);

        public void UpdateOrder(Order order)
        => OrderDAO.UpdateOrder(order);
    }
}
