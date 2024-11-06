using BusinessObject;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Customer;

public class ProductDetail : PageModel
{
    public Product Product { get; set; }
    
    private readonly IProductRepository _productRepository;

    public ProductDetail(IProductRepository productRepository)
    {
        this._productRepository = productRepository;
    }

    public void OnGet(long id)
    {
        Product = _productRepository.GetProductById(id);
    }
}