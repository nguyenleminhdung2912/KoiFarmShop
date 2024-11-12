using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Customer;
[Authorize(Roles = "Customer")]

public class CreateConsignment : PageModel
{
    private readonly IConsignmentRepository _consignmentRepository;
    private readonly IHubContext<SignalRHub> hubContext;


    public string Message { get; set; }
    [BindProperty] public string KoiName { get; set; }

    [BindProperty] public IFormFile KoiImage { get; set; }

    [BindProperty] public DateTime? FromTime { get; set; }

    [BindProperty] public DateTime? ToTime { get; set; }

    public Dictionary<string, string> ValidateErorrs { get; set; } = new Dictionary<string, string>();

    public CreateConsignment(IConsignmentRepository consignmentRepository, IHubContext<SignalRHub> hubContext)
    {
        this._consignmentRepository = consignmentRepository;
        this.hubContext = hubContext;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        byte[] koiImage = null;
        if (string.IsNullOrEmpty(KoiName))
        {
            ValidateErorrs["KoiName"] = "Koi Name không được để trống!!!";
        }
        else if (KoiImage == null)
        {
            ValidateErorrs["KoiImage"] = "Koi Image không được để trống";
        }
        else if (FromTime == null)
        {
            ValidateErorrs["FromTime"] = "FromTime không được để trống";
        }
        else if (ToTime == null)
        {
            ValidateErorrs["ToTime"] = "ToTime không được để trống";
        }
        else if (FromTime >= ToTime)
        {
            ValidateErorrs["ToTime"] = "Thời gian to time kết thúc ký gửi phải lớn hơn thời gian bắt đầu";
        }
        else if ((ToTime - FromTime).Value.Days < 1)
        {
            ValidateErorrs["ToTime"] = "Thời gian ký gửi ít nhất là 1 ngày";
        }
        else if ((FromTime - DateTime.Now).Value.Days < 7)
        {
            ValidateErorrs["FromTime"] = "Thời gian bắt đầu ký gửi phải sau lần tạo request consignment ít nhất 1 tuần";
        }
        else
        {
            Consignment consignment = new Consignment();
            consignment.ConsignmentId = GetNextConsignmentId();
            consignment.KoiName = KoiName;
            using (var memoryStream = new MemoryStream())
            {
                KoiImage.CopyTo(memoryStream);
                koiImage = memoryStream.ToArray();
            }

            consignment.ImageData = koiImage;
            consignment.FromTime = FromTime;
            consignment.ToTime = ToTime;
            consignment.IsDeleted = false;
            // consignment.UserId = long.Parse(User.FindFirst("userId").Value);
            consignment.UserId = long.Parse(User.FindFirst("userId").Value);
            consignment.CreateAt = DateTime.Now;
            consignment.Status = "PENDING";
            if (_consignmentRepository.AddConsignment(consignment))
            {
                TempData["SuccessMessage"] = "Tạo request consignment thành công!!!";
                
                hubContext.Clients.All.SendAsync("RefreshData");
                
                return RedirectToPage("/Customer/ViewConsignment");
            }
            else
            {
                Message = "Tạo request consignment thất bại!!!";
            }
        }
        hubContext.Clients.All.SendAsync("RefreshData");

        return Page();
    }

    private long GetNextConsignmentId()
    {
        // Lấy ID lớn nhất hiện tại trong cơ sở dữ liệu
        var maxId = _consignmentRepository.GetNextConsignmentId(); // Giả định bạn có phương thức này trong repository
        return maxId + 1; // Trả về ID tiếp theo
    }
}