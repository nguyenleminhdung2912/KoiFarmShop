using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository.IRepository
{
    public interface IConsignmentRepository
    {
        List<Consignment> GetConsignmentsByStatusByUser(string status, long userId);
        
        List<Consignment> GetConsignmentsByStaff();
        
        List<Consignment> GetConsignments(long userId);

        List<Consignment> GetConsignmentsByStatusByStaff(string status);
        
        Consignment GetConsignmentById(long id);
        
        bool AddConsignment(Consignment consignment);
        
        bool UpdateConsignment(Consignment consignment);
        
        bool DeleteConsignment(long id);
        
        long GetNextConsignmentId();

        bool ApproveConsignmentByStaff(Consignment consignment);
        
        bool RejectConsignmentByStaff(long consignmentId);
        
        bool ConfirmConsignmentByCustomer(long consignmentId);
        
        bool CancelConsignmentByCustomer(long consignmentId);
        
        
    }
}
