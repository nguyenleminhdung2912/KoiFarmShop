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

        public List<Product> GetProductForCustomer()
        {
	        return ProductDAO.GetProductsForCustomer();
        }

        public Product GetProductById(int id)
        {
	        return ProductDAO.GetProductById(id);
        }

        public long GetNextProductId()
        {
	        return ProductDAO.GetNextConsignmentId();
        }

        public List<Product> GetProductList()
        {
            return ProductDAO.GetProducts();
        }

		public void AddProduct(Product product)
		{
			 ProductDAO.AddProduct(product);
		}

		public bool CheckProductExistById(long productId)
		{
			if(ProductDAO.GetProductById(productId) != null)
			{
				return true;
			}
			return false;
		}

		public bool DeleteProductById(long productId)
		{
			return ProductDAO.DeleteProduct(productId);
		}

		public List<Product> SearchProductByName(string productName)
		{
			return ProductDAO.SearchProductByName(productName);
		}

		public Product GetProductById(long productId)
		{
			return ProductDAO.GetProductById(productId);
		}

		public void UpdateProduct(Product product)
		{
			 ProductDAO.UpdateProduct(product);
		}
	}
}
