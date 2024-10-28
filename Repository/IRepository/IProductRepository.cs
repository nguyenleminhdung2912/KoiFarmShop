using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IProductRepository
    {
        List<Product> GetProductsByListString(string listString);
		List<Product> GetProductList();

        bool CheckProductExistById(long productId);

        List<Product> SearchProductByName(string productName);  
        
        void UpdateProduct(Product product);    
        Product GetProductById(long productId);
        bool DeleteProductById(long productId);
        void AddProduct(Product product);
	}
}
