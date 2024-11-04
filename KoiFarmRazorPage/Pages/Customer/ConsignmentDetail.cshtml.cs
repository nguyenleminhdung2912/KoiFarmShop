using BusinessObject;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Customer;

public class ConsignmentDetail : PageModel
{
    public Consignment Consignment { get; set; } = new Consignment();
    private readonly IConsignmentRepository _consignmentRepository;

    public ConsignmentDetail(IConsignmentRepository consignmentRepository)
    {
        this._consignmentRepository = consignmentRepository;
    }
    public void OnGet(long id)
    {
        Consignment = _consignmentRepository.GetConsignmentById(id);
    }
}