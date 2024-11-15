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

        public async Task<Order> GetOrderById(long? id)
        => await OrderDAO.GetOrderById(id);

        public List<Order> GetOrdersByAccount(long accountId)
        => OrderDAO.GetOrdersByAccount(accountId);

        public List<Order> GetOrdersByFromDateToDate(DateTime fromDate, DateTime toDate)
        => OrderDAO.GetOrdersByFromDateToDate(fromDate, toDate);

        public void SaveOrder(Order order)
        => OrderDAO.SaveOrder(order);

        public List<Order> GetOrdersByShipStatus(string shipStatus)
        {
            return OrderDAO.GetOrdersByShip(shipStatus);
        }

        public bool SetShipStatusOrder(long orderId, string shipStatus)
        {
            return OrderDAO.SetShipStatusOrder(orderId, shipStatus);
        }

        public Order GetOrderByIdNotAsync(long orderId)
        {
            return OrderDAO.GetOrderByIdNotAsync(orderId);
        }

        public void UpdateOrderByCancel(Order order)
        {
            OrderDAO.UpdateOrderByCancel(order);
        }

        public async Task<bool> UpdateOrder(Order order)
        => await OrderDAO.UpdateOrder(order);
        
        public async Task<bool> CancelOrder(Order order, long userId)
            => await OrderDAO.CancelOrder(order, userId);
    }
}
