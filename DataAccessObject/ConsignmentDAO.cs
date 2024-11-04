using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject
{
    public class ConsignmentDAO
    {
        private static KoiFarmShopDatabaseContext db = new KoiFarmShopDatabaseContext();

        public static List<Consignment> GetConsignmentsByUserId(long userId)
        {
            return db.Consignments.Where(c => c.UserId == userId).ToList();
        }
        public static List<Consignment> GetConsignmentsByStatus(string status, long userId)
        {
            if (status == "ALL")
            {
                return db.Consignments.Where(c => c.UserId == userId).ToList();
            }
            
            return db.Consignments.Where(c => c.UserId == userId).Where(c => c.Status == status).ToList();  
        }

        public static Consignment GetConsignmentById(long consignmentId)
        {
            return db.Consignments.Include(c => c.User).FirstOrDefault(c => c.ConsignmentId == consignmentId);  
        }

        public static bool CreateConsignment(Consignment consignment)
        {
            db.Consignments.Add(consignment);
            db.SaveChanges();
            var existConsigment = db.Consignments.FirstOrDefault(c => c.ConsignmentId == consignment.ConsignmentId);
            if (existConsigment != null)
            {
                return true;
            }

            return false;
        }
    }
}
