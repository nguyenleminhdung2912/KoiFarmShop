using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Customer;

public class ConsignmentDetail : PageModel
{
    public Consignment Consignment { get; set; } = new Consignment();

    private readonly IConsignmentRepository _consignmentRepository;

    public string Message { get; set; }

    public ConsignmentDetail(IConsignmentRepository consignmentRepository)
    {
        this._consignmentRepository = consignmentRepository;
    }

    public void OnGet(long id)
    {
        Consignment = _consignmentRepository.GetConsignmentById(id);
    }

    public IActionResult OnPost()
    {
        string handler = Request.Form["handler"];
        if (handler == "Confirm")
        {
            if (_consignmentRepository.ConfirmConsignmentByCustomer(long.Parse(Request.Form["consignmentId"])))
            {
                TempData["SuccessMessage"] = "Consignment approved successfully!";
                return RedirectToPage("/Customer/ViewConsignment");
            }
            else
            {
                Message = "Consignment confirmation failed!";
                Consignment = _consignmentRepository.GetConsignmentById(long.Parse(Request.Form["consignmentId"]));
                return Page();
            }
        }

        if (handler == "Cancel")
        {
            if (_consignmentRepository.CancelConsignmentByCustomer(long.Parse(Request.Form["consignmentId"])))
            {
                TempData["SuccessMessage"] = "Consignment cancelled successfully!";
                return RedirectToPage("/Customer/ViewConsignment");
            }
            else
            {
                Message = "Consignment cancel failed!";
                Consignment = _consignmentRepository.GetConsignmentById(long.Parse(Request.Form["consignmentId"]));
                return Page();
            }
        }

        return Page();
    }
}