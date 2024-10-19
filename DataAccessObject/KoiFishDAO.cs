using BusinessObject;
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
