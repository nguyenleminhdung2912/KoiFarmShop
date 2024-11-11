using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccessObject;
using Repository.IRepository;
using Repository.Repository;

namespace KoiFarmRazorPage.Pages.Admin.OrderManagement
{
    public class DetailsModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IKoiFishRepository koiFishRepository;
        private readonly IProductRepository productRepository;

        public DetailsModel()
        {
            _orderRepository = new OrderRepository();
            koiFishRepository = new KoiFishRepository();
            productRepository = new ProductRepository();
        }

        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                List<Product> products = productRepository.GetProductsByListString(order.ProductId);
                List<KoiFish> koiFishs = koiFishRepository.GetKoiFishsByListString(order.KoiFishId);
                order.KoiFishList = koiFishs;
                order.ProductList = products;
                Order = order;
            }
            return Page();
        }
    }
}
