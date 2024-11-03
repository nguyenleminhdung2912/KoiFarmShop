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

        public async Task OnGetAsync()
        {
            // Lấy danh sách cá koi từ cơ sở dữ liệu
            KoiFishList = await koiFishRepository.GetAllKoiFishNotDeleted();
        }
    }
}
