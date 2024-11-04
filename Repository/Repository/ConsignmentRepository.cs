using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessObject;
using Repository.IRepository;

namespace Repository.Repository
{
    public class ConsignmentRepository : IConsignmentRepository
    {
        public List<Consignment> GetConsignmentsByStatus(string status, long userId)
        {
            return ConsignmentDAO.GetConsignmentsByStatus(status, userId);
        }

        public List<Consignment> GetConsignments(long userId)
        {
            return ConsignmentDAO.GetConsignmentsByUserId(userId);
        }

        public Consignment GetConsignmentById(long id)
        {
            return ConsignmentDAO.GetConsignmentById(id);
        }

        public bool AddConsignment(Consignment consignment)
        {
            return ConsignmentDAO.CreateConsignment(consignment);
        }
    }
}
