using BusinessObject;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Customer;

public class ViewProduct : PageModel
{
    public List<Product> Products { get; set; } = new List<Product>();
    
    private readonly IProductRepository _productRepository;

    public ViewProduct(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task OnGet()
    {
        Products = await _productRepository.GetProductForCustomer();
    }
}