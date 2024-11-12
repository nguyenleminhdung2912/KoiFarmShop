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


    public ProductDetail(IProductRepository productRepository, ICartRepository cartRepository)
    {
        _productRepository = productRepository;
        _cartRepository = cartRepository;  // Add this injection
    }

    public void OnGet(long id)
    {
        Product = _productRepository.GetProductById(id);
    }

    public IActionResult OnPostAddToCart( long productId, int quantity)
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToPage("/Auth/Login");
        }
        var product = _productRepository.GetProductById(productId);

        if (product != null)
        {
            _cartRepository.AddProduct(product, 1);
        }
       
        return RedirectToPage("/Customer/ProductDetail", new {id = productId});
    }
}