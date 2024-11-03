using BusinessObject;
using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IAdminRepository
    {
        public Task<List<Order>> GetOrdersByDateRange(DateTime startDate, DateTime endDate);
        public Task<List<Order>> GetOrdersThisMonth();
        public Task<List<Order>> GetOrdersThisWeek();
        public Task<List<Order>> GetOrdersThisYear();
        public Task<List<Order>> GetOrdersNearest4Years();
        public Task<List<Order>> GetOrdersToday();
        public Task<RevenueDTO> GetRevenueDataAsync();
    }
}
