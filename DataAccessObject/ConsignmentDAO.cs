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
            return db.Consignments.Where(c => c.UserId == userId && c.IsDeleted == false).OrderByDescending(c => c.CreateAt).ToList();
        }

        public static List<Consignment> GetConsignmentsByStatusByStaff(string status)
        {
            if (status == "ALL")
            {
                return db.Consignments.Include(c => c.User).ToList();
            }
            return db.Consignments.Include(c => c.User).Where(c => c.Status == status && c.IsDeleted == false).ToList();
        }

        public static List<Consignment> GetConsignmentsByStaff()
        {
            return db.Consignments.Include(c => c.User).Where(c => c.IsDeleted == false).ToList();
        }

        public static List<Consignment> GetConsignmentsByStatus(string status, long userId)
        {
            if (status == "ALL")
            {
                return db.Consignments.Where(c => c.UserId == userId).ToList();
            }

            return db.Consignments.Where(c => c.UserId == userId).Where(c => c.Status == status && c.IsDeleted == false).ToList();
        }

        public static Consignment GetConsignmentById(long consignmentId)
        {
            return db.Consignments.Include(c => c.User).FirstOrDefault(c => c.ConsignmentId == consignmentId);
        }

        public static bool ApproveConsignmentByStaff(Consignment consignment)
        {
            var existConsignment = db.Consignments.Find(consignment.ConsignmentId);
            if (existConsignment != null)
            {
                existConsignment.Price = consignment.Price;
                existConsignment.Status = "APPROVED";
                db.Consignments.Update(existConsignment);
                db.SaveChanges();
                return true;
            }

            return false;
        }
        
        public static bool RejectConsignmentByStaff(long consignmentId)
        {
            var existConsignment = db.Consignments.Find(consignmentId);
            if (existConsignment != null)
            {
                existConsignment.Status = "REJECTED";
                db.Consignments.Update(existConsignment);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public static bool ConfirmConsignmentByCustomer(long consignmentId)
        {
            var existConsignment = db.Consignments.Find(consignmentId);
            if (existConsignment != null)
            {
                existConsignment.Status = "CONFIRMED";
                db.Consignments.Update(existConsignment);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        
        public static bool CancelConsignmentByCustomer(long consignmentId)
        {
            var existConsignment = db.Consignments.Find(consignmentId);
            if (existConsignment != null)
            {
                existConsignment.Status = "CANCELLED";
                db.Consignments.Update(existConsignment);
                db.SaveChanges();
                return true;
            }
            return false;
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

        public static bool UpdateConsignment(Consignment consignment)
        {
            var existConsignment = db.Consignments.Find(consignment.ConsignmentId);
            if (existConsignment != null)
            {
                existConsignment.KoiName = consignment.KoiName;
                existConsignment.Price = consignment.Price;
                existConsignment.FromTime = consignment.FromTime;
                existConsignment.ToTime = consignment.ToTime;
                existConsignment.UpdateAt = consignment.UpdateAt;
                existConsignment.IsDeleted = consignment.IsDeleted;
                existConsignment.Status = consignment.Status;
                existConsignment.UserId = consignment.UserId;
                existConsignment.ImageData = consignment.ImageData;
                db.Consignments.Update(existConsignment);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public static bool DeleteConsignment(long consignmentId)
        {
            var existConsignment = db.Consignments.Find(consignmentId);
            if (existConsignment != null)
            {
                db.Consignments.Remove(existConsignment);
                db.SaveChanges();
                return true;
            }

            return false;
        }
        

        public static long GetNextConsignmentId()
        {
            // Lấy ID lớn nhất hiện có trong bảng Consignment
            var maxId = db.Consignments.Max(c => (long?)c.ConsignmentId) ?? 0;
            return maxId + 1; // Trả về ID mới
        }
    }
}