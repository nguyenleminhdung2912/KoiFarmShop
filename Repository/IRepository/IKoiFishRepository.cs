using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IKoiFishRepository
    {
        List<KoiFish> GetKoiFishsByListString(string listString);
        Task<List<KoiFish>> GetAllKoiFishNotDeleted();
        Task<KoiFish?> GetKoiFishById(long id);

        List<KoiFish> GetkoiFishes();
        
        bool CreateKoiFish(KoiFish koiFish);

        long GetKoiFishId();
        
        bool DeleteKoiFishById(long koiFishId);

        List<KoiFish> GetKoiFishByCustomerOrGuest();
        
        KoiFish GetKoiFishByIdByStaff(long koiFishId);
        
        bool UpdateKoiFish(KoiFish koiFish);
        
        List<KoiFish> GetKoiFishByName(string koiName);
    }
}
