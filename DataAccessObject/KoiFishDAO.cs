using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject
{
    public class KoiFishDAO
    {
        public static async Task<List<KoiFish>> GetAllKoiFishNotDeleted()
        {
            var list = new List<KoiFish>();
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                list = context.KoiFishes
                    .Include(kf => kf.KoiFishRatings)
                    .Where(kf => kf.IsDeleted == false)
                    .ToList();
            }
            catch (Exception ex) { }
            return list;
        }

        public static async Task<KoiFish?> GetKoiFishById(long id)
        {
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                var koiFish = await context.KoiFishes
                    .Include(kf => kf.KoiFishRatings)
                    .Where(kf => kf.IsDeleted == false && kf.KoiFishId == id) // Thêm điều kiện để tìm theo id
                    .FirstOrDefaultAsync();
                return koiFish;
            }
            catch (Exception ex)
            { // Xử lý lỗi ở đây, ví dụ: log lỗi
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static List<KoiFish> GetKoiFishsByListString(string listString)
        {
            var list = new List<KoiFish>();
            try
            {
                using var context = new KoiFarmShopDatabaseContext();
                var koiFishIds = listString
                          .Split(',')
                          .Select(id => int.Parse(id.Trim()))
                          .ToList();
                list = context.KoiFishes
                    .Where(kf => koiFishIds.Contains((int)kf.KoiFishId))
                    .ToList();
            }
            catch (Exception ex) { }
            return list;
        }
    }
}
