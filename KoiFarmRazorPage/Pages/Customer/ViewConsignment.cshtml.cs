using System.Drawing;
using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Customer;

public class ViewConsignment : PageModel
{
    public List<Consignment> Consignments { get; set; } = new List<Consignment>();
    public string SelectedStatus { get; set; } = "ALL";

    private readonly IConsignmentRepository _consignmentRepository;

    public ViewConsignment(IConsignmentRepository consignmentRepository)
    {
        this._consignmentRepository = consignmentRepository;
    }

    public IActionResult OnGet()
    {
        // if (!User.Identity.IsAuthenticated)
        // {
        //     return RedirectToPage("/Auth/Login"); 
        // }

        // long userId = long.Parse(User.FindFirst("userId").Value);
        // Consignments = _consignmentRepository.GetConsignments(userId);
        long userId = 1;
        Consignments = _consignmentRepository.GetConsignments(userId);
        return Page();
    }

    public void OnPost()
    {
        SelectedStatus = Request.Form["statusConsignment"];
        Consignments = _consignmentRepository.GetConsignmentsByStatus(SelectedStatus, 1);
        
    }
}