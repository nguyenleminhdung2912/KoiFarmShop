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
        public List<Consignment> GetConsignmentsByStatusByUser(string status, long userId)
        {
            return ConsignmentDAO.GetConsignmentsByStatus(status, userId);
        }

        public List<Consignment> GetConsignmentsByStaff()
        {
            return ConsignmentDAO.GetConsignmentsByStaff();
        }

        public List<Consignment> GetConsignments(long userId)
        {
            return ConsignmentDAO.GetConsignmentsByUserId(userId);
        }

        public List<Consignment> GetConsignmentsByStatusByStaff(string status)
        {
            return ConsignmentDAO.GetConsignmentsByStatusByStaff(status);
        }

        public Consignment GetConsignmentById(long id)
        {
            return ConsignmentDAO.GetConsignmentById(id);
        }

        public bool AddConsignment(Consignment consignment)
        {
            return ConsignmentDAO.CreateConsignment(consignment);
        }

        public bool UpdateConsignment(Consignment consignment)
        {
            return ConsignmentDAO.UpdateConsignment(consignment);
        }

        public bool DeleteConsignment(long id)
        {
            return ConsignmentDAO.DeleteConsignment(id);
        }

        public long GetNextConsignmentId()
        {
            return ConsignmentDAO.GetNextConsignmentId();
        }

        public bool ApproveConsignmentByStaff(Consignment consignment)
        {
            return ConsignmentDAO.ApproveConsignmentByStaff(consignment);
        }

        public bool RejectConsignmentByStaff(long consignmentId)
        {
            return ConsignmentDAO.RejectConsignmentByStaff(consignmentId);
        }

        public bool ConfirmConsignmentByCustomer(long consignmentId)
        {
            return ConsignmentDAO.ConfirmConsignmentByCustomer(consignmentId);
        }

        public bool CancelConsignmentByCustomer(long consignmentId)
        {
            return ConsignmentDAO.CancelConsignmentByCustomer(consignmentId);
        }
        
    }
}
