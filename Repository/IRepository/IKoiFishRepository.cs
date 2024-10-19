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
    }
}
