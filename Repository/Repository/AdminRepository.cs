using BusinessObject;
using BusinessObject.DTO;
using DataAccessObject;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class AdminRepository : IAdminRepository
    {
        public Task<List<Order>> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        => OrderDAO.GetOrdersByDateRangeAsync(startDate, endDate);

        public Task<List<Order>> GetOrdersThisMonth()
        => OrderDAO.GetOrdersThisMonthAsync();

        public Task<List<Order>> GetOrdersThisWeek()
        => OrderDAO.GetOrdersThisWeekAsync();

        public Task<List<Order>> GetOrdersThisYear()
        => OrderDAO.GetOrdersThisYearAsync();
        public Task<List<Order>> GetOrdersNearest4Years()
        => OrderDAO.GetOrdersNearest4Years();

        public Task<List<Order>> GetOrdersToday()
        => OrderDAO.GetOrdersToday();

        public Task<RevenueDTO> GetRevenueDataAsync()
        => OrderDAO.GetRevenueDataAsync();
    }
}
