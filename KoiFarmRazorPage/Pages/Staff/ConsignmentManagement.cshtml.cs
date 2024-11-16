using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff;

[Authorize(Roles = "Staff")]

public class ConsignmentManagement : PageModel
{
    public string SelectedStatus { get; set; } = "ALL";

    public List<Consignment> Consignments { get; set; } = new List<Consignment>();

    private readonly IConsignmentRepository _consignmentRepository;
    private readonly IHubContext<SignalRHub> hubContext;

    public string Message { get; set; }

    public ConsignmentManagement(IConsignmentRepository consignmentRepository, IHubContext<SignalRHub> hubContext)
    {
        this._consignmentRepository = consignmentRepository;
        this.hubContext = hubContext;
    }

    public void OnGet()
    {
        Consignments = _consignmentRepository.GetConsignmentsByStaff();
    }

    public IActionResult OnPost()
    {
        SelectedStatus = Request.Form["statusConsignment"];
        string handler = Request.Form["handler"];
        if (handler == "Search")
        {
            Consignments = _consignmentRepository.GetConsignmentsByStatusByStaff(SelectedStatus);
        }

        if (handler == "Approve")
        {
            if (string.IsNullOrEmpty(Request.Form["selectedConsignmentId"]))
            {
                SelectedStatus = "PENDING";
                Message = "Hay chon consignment cu the de approve";
                Consignments = _consignmentRepository.GetConsignmentsByStatusByStaff(SelectedStatus);
            }
            else
            {
                return RedirectToPage("/Staff/ApproveConsignment",
                    new { consignmentId = long.Parse(Request.Form["selectedConsignmentId"]) });
            }
        }

        if (handler == "Reject")
        {
            if (string.IsNullOrEmpty(Request.Form["selectedConsignmentId"]))
            {
                SelectedStatus = "PENDING";
                Message = "Hay chon consignment cu the de reject";
                Consignments = _consignmentRepository.GetConsignmentsByStatusByStaff(SelectedStatus);
            }
            else
            {
                if(_consignmentRepository.RejectConsignmentByStaff(long.Parse(Request.Form["selectedConsignmentId"])))
                {
                    TempData["SuccessMessage"] = "Reject consignment thành công!!!";
                    Consignments = _consignmentRepository.GetConsignmentsByStaff();
                    return Page();
                }else
                {
                    Message = "Reject consignment failed";
                    List<Consignment> consignments = new List<Consignment>();
                    consignments.Add(_consignmentRepository.GetConsignmentById(long.Parse(Request.Form["selectedConsignmentId"])));
                    Consignments = consignments;
                }
            }
        }
        
        // hubContext.Clients.All.SendAsync("RefreshData");

        return Page();
    }
}