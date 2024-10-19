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
    public class ProductRepository : IProductRepository
    {
        public List<Product> GetProductsByListString(string listString)
        => ProductDAO.GetProductsByListString(listString);
    }
}
