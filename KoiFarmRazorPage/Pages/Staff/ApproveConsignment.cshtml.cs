using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff;

public class ApproveConsignment : PageModel
{
    [BindProperty] public Consignment Consignment { get; set; }

    private readonly IConsignmentRepository _consignmentRepository;

    public Dictionary<string, string> ValidateErrors { get; set; } = new Dictionary<string, string>();

    public string Message { get; set; }

    public ApproveConsignment(IConsignmentRepository consignmentRepository)
    {
        this._consignmentRepository = consignmentRepository;
    }

    public void OnGet(long consignmentId)
    {
        Consignment = _consignmentRepository.GetConsignmentById(consignmentId);
    }

    public IActionResult OnPost()
    {
        if (Consignment.Price <= 0 || Consignment.Price == null)
        {
            ValidateErrors["ConsignmentPrice"] = "Price khong duoc de trong va phai lon hon 0";
            Consignment = _consignmentRepository.GetConsignmentById(long.Parse(Request.Form["consignmentId"]));
            return Page();
        }
        else
        {
            Consignment.ConsignmentId = long.Parse(Request.Form["consignmentId"]);
            if (_consignmentRepository.ApproveConsignmentByStaff(Consignment))
            {
                TempData["SuccessMessage"] = "Approve consignment thành công!!!";
                return RedirectToPage("/Staff/ConsignmentManagement");
            }
            else
            {
                Message = "Consignment approval failed";
                return Page();
            }
        }
    }
}