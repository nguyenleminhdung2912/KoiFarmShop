using System.Drawing;
using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Customer;

public class ViewConsignment : PageModel
{
    public string Message { get; set; }
    public List<Consignment> Consignments { get; set; } = new List<Consignment>();
    public string SelectedStatus { get; set; } = "ALL";

    private readonly IConsignmentRepository _consignmentRepository;

    public ViewConsignment(IConsignmentRepository consignmentRepository)
    {
        this._consignmentRepository = consignmentRepository;
    }

    public IActionResult OnGet()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToPage("/Auth/Login"); 
        }
        else
        {
            long userId = long.Parse(User.FindFirst("userId").Value);
            Consignments = _consignmentRepository.GetConsignments(userId);
            return Page();
        }
    }

    public IActionResult OnPost()
    {
        SelectedStatus = Request.Form["statusConsignment"];
        Consignments = _consignmentRepository.GetConsignmentsByStatusByUser(SelectedStatus, long.Parse(User.FindFirst("userId").Value));
        string handler = Request.Form["handler"];
        if (handler == "Confirm")
        {
            if(_consignmentRepository.ConfirmConsignmentByCustomer(long.Parse(Request.Form["consignmentId"])))
            {
                TempData["SuccessMessage"] = "Consignment approved successfully!";
                Consignments = _consignmentRepository.GetConsignments(long.Parse(User.FindFirst("userId").Value));
                return Page();
            }else
            {
                Message = "Consignment confirmation failed!";
                Consignments = _consignmentRepository.GetConsignments(long.Parse(User.FindFirst("userId").Value));
                return Page();
            }
        }

        if (handler == "Cancel")
        {
            if(_consignmentRepository.CancelConsignmentByCustomer(long.Parse(Request.Form["consignmentId"])))
            {
                TempData["SuccessMessage"] = "Consignment cancelled successfully!";
                Consignments = _consignmentRepository.GetConsignments(long.Parse(User.FindFirst("userId").Value));
                return Page();
            }else
            {
                Message = "Consignment cancel failed!";
                Consignments = _consignmentRepository.GetConsignments(long.Parse(User.FindFirst("userId").Value));
                return Page();
            }
        }

        return Page();

    }
    
    [IgnoreAntiforgeryToken] 
    public JsonResult OnPostDelete(long id)
    {
        TempData["DeleteSuccessful"] = "Delete consignment successfull";
        bool isDeleted = _consignmentRepository.DeleteConsignment(id);
    
        
        return new JsonResult(new { success = isDeleted });
    }
}