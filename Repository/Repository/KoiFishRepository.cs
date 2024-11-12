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
    public class KoiFishRepository : IKoiFishRepository
    {
        public Task<List<KoiFish>> GetAllKoiFishNotDeleted()
        => KoiFishDAO.GetAllKoiFishNotDeleted();

        public Task<KoiFish?> GetKoiFishById(long id)
        => KoiFishDAO.GetKoiFishById(id);

        public List<KoiFish> GetkoiFishes()
        {
            return KoiFishDAO.GetFishes();
        }

        public bool CreateKoiFish(KoiFish koiFish)
        {
            return KoiFishDAO.CreateKoiFish(koiFish);
        }

        public long GetKoiFishId() => KoiFishDAO.GetNextKoiFishId();
        public bool DeleteKoiFishById(long koiFishId) => KoiFishDAO.DeleteKoiFishById(koiFishId);
        public List<KoiFish> GetKoiFishByCustomerOrGuest() => KoiFishDAO.GetKoiFishByCustomerOrGuest();
        public KoiFish GetKoiFishByIdByStaff(long koiFishId) => KoiFishDAO.GetKoiFishByIdByStaff(koiFishId);
        public bool UpdateKoiFish(KoiFish koiFish) => KoiFishDAO.UpdateKoiFish(koiFish);
        public List<KoiFish> GetKoiFishByName(string koiName)
        {
            return KoiFishDAO.SearchKoiFishByName(koiName);
        }

        public List<KoiFish> GetKoiFishsByListString(string listString)
        => KoiFishDAO.GetKoiFishsByListString(listString);
    }
}
