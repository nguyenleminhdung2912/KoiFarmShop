using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Customer;

public class EditConsignment : PageModel
{
    private readonly IConsignmentRepository _consignmentRepository;

    public string Message { get; set; }

    [BindProperty] public Consignment Consignment { get; set; }

    public Dictionary<string, string> ValidateErrors { get; set; } = new Dictionary<string, string>();


    [BindProperty] public IFormFile? KoiImage { get; set; }

    public EditConsignment(IConsignmentRepository consignmentRepository)
    {
        this._consignmentRepository = consignmentRepository;
    }

    public void OnGet(long id)
    {
        Consignment = _consignmentRepository.GetConsignmentById(id);
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(Consignment.KoiName))
        {
            ValidateErrors["KoiName"] = "KoiName không được để trống";
        }
        // else if (KoiImage == null)
        // {
        //     ValidateErrors["KoiImage"] = "Koi image không đuợc để trống";
        // }
        else if (Consignment.FromTime == null)
        {
            ValidateErrors["FromTime"] = "FromTime không đuợc để trống";
        }
        else if (Consignment.ToTime == null)
        {
            ValidateErrors["ToTime"] = "ToTime không được để trống";
        }
        else if ((Consignment.ToTime - Consignment.FromTime).Value.Days < 1)
        {
            ValidateErrors["ToTime"] = "Thời gian ký gửi từ fromtime đến totime phải ít nhất 1 ngày";
        }
        else if (Consignment.FromTime >= Consignment.ToTime)
        {
            ValidateErrors["ToTime"] = "Thời gian to time kết thúc ký gửi phải lớn hơn thời gian bắt đầu";
        }
        else if ((Consignment.ToTime - DateTime.Now)?.TotalDays < 7)
        {
            ValidateErrors["FromTime"] =
                "Thời gian bắt đầu ký gửi của cập nhat ky gui phai sau thoi gian cap nhat 1 tuan ";
        }
        else
        {
            Consignment.ConsignmentId = long.Parse(Request.Form["consignmentId"]);
            if (KoiImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    KoiImage.CopyTo(memoryStream);
                    Consignment.ImageData = memoryStream.ToArray();
                }
            }

            // Consignment.CreateAt = DateTime.Parse(Request.Form["createAt"]);
            Consignment.UpdateAt = DateTime.Now;
            Consignment.IsDeleted = false;
            Consignment.Status = "PENDING";
            Consignment.UserId = long.Parse(User.FindFirst("userId").Value);
            if (_consignmentRepository.UpdateConsignment(Consignment))
            {
                TempData["SuccessMessage"] = "Cập nhật consignment thành công!!!";
                return RedirectToPage("/Customer/ViewConsignment");
            }
            else
            {
                Message = "Consignment updated fail";
            }
        }

        Consignment = _consignmentRepository.GetConsignmentById(long.Parse(Request.Form["consignmentId"]));
        return Page();
    }
}