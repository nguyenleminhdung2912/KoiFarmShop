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
        List<Consignment> GetConsignmentsByStatus(string status, long userId);
        
        List<Consignment> GetConsignments(long userId);
        
        Consignment GetConsignmentById(long id);
        
        bool AddConsignment(Consignment consignment);
    }
}
