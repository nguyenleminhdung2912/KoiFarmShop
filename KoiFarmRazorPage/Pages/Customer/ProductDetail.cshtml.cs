using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;
using Repository.Repository;

namespace KoiFarmRazorPage.Pages.Customer;

public class ProductDetail : PageModel
{
    public Product Product { get; set; }
    
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;

    public ProductDetail(IProductRepository productRepository)
    {
        this._productRepository = productRepository;
    }

    public void OnGet(long id)
    {
        Product = _productRepository.GetProductById(id);
    }

    public void OnGet(int productId)
    {
        Product = _productRepository.GetProductById(productId); // Load product using ProductRepository
    }
    public IActionResult OnPostAddToCart( long productId, int quantity)
    {
        var product = _productRepository.GetProductById(productId);

        if (product != null)
        {
            _cartRepository.AddProduct(product, quantity);
        }

        return Page(); 
    }
}