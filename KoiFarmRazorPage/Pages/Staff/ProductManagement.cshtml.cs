using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff
{
    [Authorize(Roles = "Staff")]

    public class ProductManagementModel : PageModel
    {
        public List<Product> Products { get; set; }

        private readonly IProductRepository productRepository;
        private readonly IHubContext<SignalRHub> hubContext;


        public string Message { get; set; }

        public ProductManagementModel(IProductRepository productRepository, IHubContext<SignalRHub> hubContext)
        {
            this.productRepository = productRepository;
            this.hubContext = hubContext;
        }

        public void OnGet()
        {
            Products = productRepository.GetProductList();
        }

        public IActionResult OnPost()
        {
            string handler = Request.Form["handler"];
            if (handler == "Search")
            {
                if (string.IsNullOrEmpty(Request.Form["productName"]))
                {
                    Products = productRepository.GetProductList();
                }
                else
                {
                    Products = productRepository.SearchProductByName(Request.Form["productName"]);
                    if (Products.IsNullOrEmpty())
                    {
                        TempData["SearchFail"] = "Product with name doesn't exist";
                        Products = productRepository.GetProductList();
                    }
                    else
                    {
                        TempData["SearchSuccess"] = "Search successful with product name";
                        Products = productRepository.SearchProductByName(Request.Form["productName"]);
                    }
                }
            }

            if (handler == "Create")
            {
                return RedirectToPage("/Staff/CreateProduct");
            }

            if (handler == "Update")
            {
                if (string.IsNullOrEmpty(Request.Form["selectedProductId"]))
                {
                    ModelState.AddModelError(string.Empty, "Hãy chọn một product cụ thể trong danh sách để cập nhật");
                    Products = productRepository.GetProductList();
                }
                else
                {
                    return RedirectToPage("/Staff/UpdateProduct",
                        new { productId = long.Parse(Request.Form["selectedProductId"]) });
                }
            }

            if (handler == "Delete")
            {
                if (string.IsNullOrEmpty(Request.Form["selectedProductId"]))
                {
                    ModelState.AddModelError(string.Empty, "Hãy chọn một product cụ thể trong danh sách để xoá");
                    Products = productRepository.GetProductList();
                }
                else
                {
                    long selectedProductId = long.Parse(Request.Form["selectedProductId"]);
                    if (productRepository.DeleteProductById(selectedProductId))
                    {
                       TempData["DeleteSuccess"] = "Xoá product thành công!!!";
                        Products = productRepository.GetProductList();
                    }
                    else
                    {
                        TempData["DeleteFail"] = "Xoá product thất bại";
                        Products = productRepository.GetProductList();
                    }
                }
            }
            // hubContext.Clients.All.SendAsync("RefreshData");

            return Page();
        }
    }
}