﻿using BusinessObject;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;
using Repository.Repository;

namespace KoiFarmRazorPage.Pages.Customer
{
    public class KoiFishDetailModel : PageModel
    {
        private readonly IKoiFishRepository koiFishRepository;
        private readonly ICartRepository _cartRepository;

        public KoiFishDetailModel()
        {
            koiFishRepository = new KoiFishRepository();
        }

        public KoiFish KoiFish { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            // Lấy thông tin cá koi theo ID
            KoiFish = await koiFishRepository.GetKoiFishById(id);

            // Nếu cá koi không tồn tại, trả về NotFound
            if (KoiFish == null)
            {
                return NotFound();
            }

            return Page();
        }
        public IActionResult OnPostAddToCart(long koiFishId)
        {
            var koiFish = koiFishRepository.GetKoiFishById(koiFishId);

            if (koiFish != null)
            {
                _cartRepository.AddKoiFish(koiFish, quantity: 1);
            }

            return Page();
        }
    }
}
