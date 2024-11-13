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

namespace KoiFarmRazorPage.Pages.Customer
{
    public class ViewKoiFishModel : PageModel
    {

        private readonly IKoiFishRepository koiFishRepository;

        public ViewKoiFishModel()
        {
            koiFishRepository = new KoiFishRepository();
        }

        public List<KoiFish> KoiFishList { get; set; } = new();
        
        [BindProperty]
        public List<int> SelectedKoiFishIds { get; set; }

        public async Task OnGetAsync()
        {
            // Lấy danh sách cá koi từ cơ sở dữ liệu
            KoiFishList = await koiFishRepository.GetAllKoiFishNotDeleted();
        }
        
        public async Task<IActionResult> OnPostCompareAsync(long[] selectedKoiFishIds)
        {
            // Kiểm tra nếu số lượng cá Koi được chọn không phải là 2
            if (selectedKoiFishIds.Length != 2)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn đúng 2 cá Koi để so sánh.");
                await OnGetAsync(); // Tải lại danh sách cá Koi nếu có lỗi
                return Page();
            }

            // Chuyển hướng đến trang CompareKoiFishes với các ID được chọn
            return RedirectToPage("/Customer/CompareKoiFishes", new { selectedKoiFishIds = selectedKoiFishIds });
        }
    }
}
