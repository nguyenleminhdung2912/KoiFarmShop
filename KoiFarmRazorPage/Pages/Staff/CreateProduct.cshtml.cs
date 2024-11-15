﻿using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;


namespace KoiFarmRazorPage.Pages.Staff
{
	[Authorize(Roles = "Staff")]

	public class CreateProductModel : PageModel
	{
		private readonly IProductRepository productRepository;
		private readonly IHubContext<SignalRHub> hubContext;

        public string Message { get; set; }

        public Dictionary<string, string> ValidateErrors { get; set; } = new Dictionary<string, string>();	
		public CreateProductModel(IProductRepository productRepository, IHubContext<SignalRHub> hubContext)
		{
			this.productRepository = productRepository;
			this.hubContext = hubContext;
		}
		public void OnGet()
		{
		}

		public IActionResult OnPost()
		{
			Product product = new Product();
			var productImage = Request.Form.Files["productImage"];
			byte[] imageBytes = null;

			// if (string.IsNullOrEmpty(Request.Form["productId"]))
			// {
			// 	ValidateErrors["ProductId"] = "Product Id không được để trống";
			// }
			// else if (productRepository.CheckProductExistById(long.Parse(Request.Form["productId"])))
			// {
			// 	ValidateErrors["ProductId"] = "Product Id đã tồn tại";
			// }
			// else 
			if (string.IsNullOrEmpty(Request.Form["productName"]))
			{
				ValidateErrors["ProductName"] = "Product name không được để trống";
			}
			else if (string.IsNullOrEmpty(Request.Form["productType"]))
			{
				ValidateErrors["ProductType"] = "Product type không được để trống";
			}
			else if (productImage == null || productImage.Length == 0)
			{
				ValidateErrors["ProductImage"] = "Product image không được để trống";
			}
			else if (string.IsNullOrEmpty(Request.Form["productPrice"]))
			{
				ValidateErrors["ProductPrice"] = "Product Price không được để trống";
			}
			// else if (string.IsNullOrEmpty(Request.Form["productStatus"]))
			// {
			// 	ValidateErrors["ProductStatus"] = "Product Status không được để trống";
			// }
			else if (string.IsNullOrEmpty(Request.Form["productQuantity"]))
			{
				ValidateErrors["ProductQuantity"] = "Product Quantity khong duoc de trong";
			}
			else
			{
				// try
				// {
				// 	product.ProductId = long.Parse(Request.Form["productId"]);
				// }
				// catch (Exception ex)
				// {
				// 	ValidateErrors["ProductId"] = "Product Id phải là số nguyên";
				// }

				product.ProductId = GetNextProductId();

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
				else if (productImage == null || productImage.Length == 0)
				{
					ValidateErrors["ProductImage"] = "Product image không được để trống";
				}

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
					if (product.Quantity <= 0)
					{
						ValidateErrors["ProductQuantity"] = "Quantity phải lớn hơn 0";
					}
				}
				catch (Exception e)
				{
					ValidateErrors["ProductQuantity"] = "Product Quantity phai la so nguyen";
				}
				product.CreateAt = DateTime.Now;
				product.Status = "Available";
				product.IsDeleted = false;
				productRepository.AddProduct(product);
				TempData["SuccessMessage"] = "Tao product thành công!!!";

				return RedirectToPage("/Staff/ProductManagement");
			}
			return Page();
		}
		private long GetNextProductId()
		{
			// Lấy ID lớn nhất hiện tại trong cơ sở dữ liệu
			var maxId = productRepository.GetNextProductId(); // Giả định bạn có phương thức này trong repository
			return maxId + 1; // Trả về ID tiếp theo
		}
	}
}
