using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff
{
    public class UpdateProductModel : PageModel
    {
        public Product Product { get; set; } = new Product();

        public string Message { get; set; }

        private readonly IProductRepository productRepository;
        private readonly IHubContext<SignalRHub> hubContext;


        public Dictionary<string, string> ValidateErrors { get; set; } = new Dictionary<string, string>();

        public UpdateProductModel(IProductRepository productRepository, IHubContext<SignalRHub> hubContext)
        {
            this.productRepository = productRepository;
        }

        public void OnGet(long productId)
        {
            Product = productRepository.GetProductById(productId);
        }

        public IActionResult OnPost()
        {
            Product product = new Product();
            var productImage = Request.Form.Files["productImage"];
            byte[] imageBytes = null;
            if (string.IsNullOrEmpty(Request.Form["productName"]))
            {
                ValidateErrors["ProductName"] = "Product name không được để trống";
            }
            else if (string.IsNullOrEmpty(Request.Form["productType"]))
            {
                ValidateErrors["ProductType"] = "Product type không được để trống";
            }
            // else if (productImage == null || productImage.Length == 0)
            // {
            //     ValidateErrors["ProductImage"] = "Product image không được để trống";
            // }
            else if (string.IsNullOrEmpty(Request.Form["productPrice"]))
            {
                ValidateErrors["ProductPrice"] = "Product Price không được để trống";
            }
            else if (string.IsNullOrEmpty(Request.Form["productStatus"]))
            {
                ValidateErrors["ProductStatus"] = "Product Status không được để trống";
            }
            else if (string.IsNullOrEmpty(Request.Form["productQuantity"]))
            {
                ValidateErrors["ProductQuantity"] = "Product Quantity không được để trống";
            }
            else
            {
                product.ProductId = long.Parse(Request.Form["productId"]);

                product.Name = Request.Form["productName"];
                product.Type = Request.Form["productType"];

                if (productImage != null && productImage.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        productImage.CopyTo(memoryStream);
                        imageBytes = memoryStream.ToArray();
                    }

                    product.ImageData = imageBytes;
                }
                // else if (productImage == null || productImage.Length == 0)
                // {
                //     ValidateErrors["ProductImage"] = "Product image không được để trống";
                // }

                try
                {
                    product.Price = double.Parse(Request.Form["productPrice"]);
                }
                catch (Exception ex)
                {
                    ValidateErrors["ProductPrice"] = "Product price phải là số";
                }

                try
                {
                    product.Quantity = int.Parse(Request.Form["productQuantity"]);
                }
                catch (Exception ex)
                {
                    ValidateErrors["ProductQuantity"] = "Product Quantity phải là số nguyên";
                }

                product.UpdateAt = DateTime.Now;
                product.Status = Request.Form["productStatus"];
                product.IsDeleted = false;
                productRepository.UpdateProduct(product);
                Message = "Cập nhật product thành công";
                Product = productRepository.GetProductById(product.ProductId);
                TempData["SuccessMessage"] = "Cap nhat product thành công!!!";
                return RedirectToPage("/Staff/ProductManagement");
            }

            Product = productRepository.GetProductById(long.Parse(Request.Form["productId"]));
            hubContext.Clients.All.SendAsync("RefreshData");

            return Page();
        }
    }
}