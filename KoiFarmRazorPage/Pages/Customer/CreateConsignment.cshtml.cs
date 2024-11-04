using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Customer;

public class CreateConsignment : PageModel
{
    private readonly IConsignmentRepository _consignmentRepository;
    
    [BindProperty] public string KoiName { get; set; }
    
    [BindProperty] public  IFormFile KoiImage { get; set; }
    
    [BindProperty] public DateTime? FromTime { get; set; }
    
    [BindProperty] public DateTime? ToTime { get; set; }

    public Dictionary<string, string> ValidateErorrs { get; set; } = new Dictionary<string, string>();

    public CreateConsignment(IConsignmentRepository consignmentRepository)
    {
        this._consignmentRepository = consignmentRepository;    
    }
    public void OnGet()
    {
        
    }

    public IActionResult OnPost()
    {
        byte [] koiImage = null;
        if (string.IsNullOrEmpty(KoiName))
        {
            ValidateErorrs["KoiName"] = "Koi Name không được để trống!!!";
        }else if (KoiImage == null)
        {
            ValidateErorrs["KoiImage"] = "Koi Image không được để trống";
        }else if (FromTime == null)
        {
            ValidateErorrs["FromTime"] = "FromTime không được để trống";
        }else if (ToTime == null)
        {
            ValidateErorrs["ToTime"] = "ToTime không được để trống";
        }else if (FromTime <= ToTime)
        {
            ValidateErorrs["ToTime"] = "ToTime không được trùng";
        }else if ((ToTime - FromTime).Value.Days < 1)
        {
            ValidateErorrs["ToTime"] = "Thời gian ký gửi ít nhất là 1 ngày";
        }else if ((FromTime - DateTime.Now).Value.Days < 7)
        {
            ValidateErorrs["FromTime"] = "Thời gian bắt đầu ký gửi phải sau lần tạo request consignment ít nhất 1 tuần";
        }
        else
        {
            Consignment consignment = new Consignment();
            consignment.KoiName = KoiName;
            using (var memoryStream = new MemoryStream())
            {
                KoiImage.CopyTo(memoryStream);
                 koiImage = memoryStream.ToArray();
            }
            consignment.ImageData = koiImage;
            consignment.FromTime = FromTime;
            consignment.ToTime = ToTime;
            // consignment.UserId = long.Parse(User.FindFirst("userId").Value);
            consignment.UserId = 2;
            consignment.CreateAt = DateTime.Now;
            consignment.Status = "PENDING";
        }

        return Page();
    }
}