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
        private static KoiFarmShopDatabaseContext db;

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
            catch (Exception ex)
            {
            }

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
            {
                // Xử lý lỗi ở đây, ví dụ: log lỗi
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

                // Chia chuỗi theo dấu phẩy, loại bỏ khoảng trắng và parse thành số
                var koiFishIds = listString
                    .Split(',')
                    .Select(id => int.Parse(id.Trim()))
                    .ToList();

                // Lấy danh sách cá Koi từ cơ sở dữ liệu mà ID có trong list
                var koiFishes = context.KoiFishes
                    .Where(kf => koiFishIds.Contains((int)kf.KoiFishId))
                    .ToList();

                // Đảm bảo rằng mỗi ID xuất hiện trong chuỗi sẽ được thêm vào danh sách đúng số lần
                list = koiFishIds
                    .Select(id => koiFishes.FirstOrDefault(kf => (int)kf.KoiFishId == id))
                    .Where(kf => kf != null)
                    .ToList();
            }
            catch (Exception ex)
            {
                // Lỗi sẽ được ghi lại ở đây nếu cần
                Console.WriteLine(ex.Message);
            }

            return list;
        }

        public static List<KoiFish> GetFishes()
        {
            db = new KoiFarmShopDatabaseContext();
            return db.KoiFishes.Include(k => k.KoiFishRatings).OrderByDescending(k => k.CreateAt).Where(k => k.IsDeleted == false).ToList();
        }

        public static bool CreateKoiFish(KoiFish koiFish)
        {
            try
            {
                db = new KoiFarmShopDatabaseContext();
                db.KoiFishes.Add(koiFish);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<KoiFish> GetKoiFishByCustomerOrGuest()
        {
            db = new KoiFarmShopDatabaseContext();
            return db.KoiFishes.Include(k => k.KoiFishRatings).Where(k => k.IsDeleted == false).ToList();
        }

        public static bool DeleteKoiFishById(long koiFishId)
        {
            db = new KoiFarmShopDatabaseContext();
            var existingKoiFish = db.KoiFishes.Find(koiFishId);
            if (existingKoiFish != null)
            {
                try
                {
                    existingKoiFish.IsDeleted = true;
                    db.KoiFishes.Update(existingKoiFish);
                    // db.KoiFishes.Remove(existingKoiFish);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return false;
        }

        public static long GetNextKoiFishId()
        {
            db = new KoiFarmShopDatabaseContext();
            var maxId = db.KoiFishes.Max(k => (long?)k.KoiFishId) ?? 0;
            return maxId + 1;
        }

        public static KoiFish GetKoiFishByIdByStaff(long koiFishId)
        {
            db = new KoiFarmShopDatabaseContext();
            return db.KoiFishes.Find(koiFishId);
        }

        public static bool UpdateKoiFish(KoiFish koiFish)
        {
            db = new KoiFarmShopDatabaseContext();
            var existKoifish = db.KoiFishes.Find(koiFish.KoiFishId);
            if (existKoifish != null)
            {
                try
                {
                    existKoifish.Name = koiFish.Name;
                    existKoifish.Origin = koiFish.Origin;
                    existKoifish.Gender = koiFish.Gender;
                    existKoifish.Age = koiFish.Age;
                    existKoifish.Size = koiFish.Size;
                    existKoifish.Breed = koiFish.Breed;
                    existKoifish.FilterRatio = koiFish.FilterRatio;
                    existKoifish.Price = koiFish.Price;
                    existKoifish.UpdateAt = koiFish.UpdateAt;
                    existKoifish.IsDeleted = koiFish.IsDeleted;
                    if (koiFish.ImageData != null)
                    {
                        existKoifish.ImageData = koiFish.ImageData;
                    }
                    existKoifish.Color = koiFish.Color;
                    db.KoiFishes.Update(existKoifish);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return false;
        }

        public static List<KoiFish> SearchKoiFishByName(string koiName)
        {
            db = new KoiFarmShopDatabaseContext();
            return db.KoiFishes.Include(k => k.KoiFishRatings).Where(k => k.Name.Contains(koiName) && k.IsDeleted == false).Take(1).ToList();
        }
    }
}