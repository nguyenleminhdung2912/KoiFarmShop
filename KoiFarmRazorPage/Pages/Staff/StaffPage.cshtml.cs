using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff
{
    public class StaffPageModel : PageModel
    {
        private readonly IConsignmentRepository _consignmentRepository;

        public List<Consignment> PendingConsignments { get; private set; } = new List<Consignment>();


        public StaffPageModel(IConsignmentRepository consignmentRepository)
        {
            this._consignmentRepository = consignmentRepository;
        }

        public void OnGet()
        {
            PendingConsignments = _consignmentRepository.GetConsignmentsByStatusByStaff("PENDING");
        }
    }
}