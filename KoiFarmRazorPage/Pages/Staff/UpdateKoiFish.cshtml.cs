using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff;

[Authorize(Roles = "Staff")]
public class UpdateKoiFish : PageModel
{
    private readonly IKoiFishRepository _koiFishRepository;
    private readonly IHubContext<SignalRHub> hubContext;


    public Dictionary<string, string> ValidateErrors { get; set; } = new Dictionary<string, string>();

    public UpdateKoiFish(IKoiFishRepository koiFishRepository, IHubContext<SignalRHub> hubContext)
    {
        this._koiFishRepository = koiFishRepository;
        this.hubContext = hubContext;
    }

    public KoiFish Koifish { get; set; } = new KoiFish();
    public string Message { get; set; }

    public void OnGet(long id)
    {
        Koifish = _koiFishRepository.GetKoiFishByIdByStaff(id);
    }

    public IActionResult OnPost()
    {
        var koiImage = Request.Form.Files["koiImage"];
        if (string.IsNullOrEmpty(Request.Form["koiName"]))
        {
            ValidateErrors["KoiName"] = "Tên cá Koi là bắt buộc";
        }
// else if (koiImage == null)
// {
//     ValidateErrors["KoiImage"] = "Hình ảnh cá Koi là bắt buộc";
// }
        else if (string.IsNullOrEmpty(Request.Form["koiOrigin"]))
        {
            ValidateErrors["KoiOrigin"] = "Nguồn gốc là bắt buộc";
        }
        else if (string.IsNullOrEmpty(Request.Form["koiGender"]))
        {
            ValidateErrors["KoiGender"] = "Giới tính cá Koi là bắt buộc";
        }
        else if (string.IsNullOrEmpty(Request.Form["koiAge"]))
        {
            ValidateErrors["KoiAge"] = "Tuổi cá Koi là bắt buộc";
        }
        else if (string.IsNullOrEmpty(Request.Form["koiSize"]))
        {
            ValidateErrors["KoiSize"] = "Kích thước cá Koi là bắt buộc";
        }
        else if (string.IsNullOrEmpty(Request.Form["koiBreed"]))
        {
            ValidateErrors["Breed"] = "Giống cá Koi là bắt buộc";
        }
        else if (string.IsNullOrEmpty(Request.Form["filterRatio"]))
        {
            ValidateErrors["FilterRatio"] = "Tỷ lệ lọc là bắt buộc";
        }
        else if (string.IsNullOrEmpty(Request.Form["koiPrice"]))
        {
            ValidateErrors["KoiPrice"] = "Giá cá Koi là bắt buộc";
        }
// else if (string.IsNullOrEmpty(Request.Form["koiStatus"]))
// {
//     ValidateErrors["KoiStatus"] = "Trạng thái là bắt buộc";
// }
        else if (string.IsNullOrEmpty(Request.Form["koiColor"]))
        {
            ValidateErrors["KoiColor"] = "Màu sắc là bắt buộc";
        }
        else if (int.Parse(Request.Form["koiAge"]) <= 0)
        {
            ValidateErrors["KoiAge"] = "Tuổi cá Koi phải lớn hơn 0";
        }
        else if (int.Parse(Request.Form["koiSize"]) <= 0)
        {
            ValidateErrors["KoiSize"] = "Kích thước cá Koi phải lớn hơn 0";
        }
        else if (double.Parse(Request.Form["koiPrice"]) <= 0)
        {
            ValidateErrors["KoiPrice"] = "Giá cá Koi phải lớn hơn 0";
        }
        else if (double.Parse(Request.Form["filterRatio"]) <= 0)
        {
            ValidateErrors["FilterRatio"] = "Tỷ lệ lọc cá Koi phải lớn hơn 0";
        }
        else
        {
            byte[] imageBytes = null;
            KoiFish koiFish = new KoiFish();
            koiFish.KoiFishId = long.Parse(Request.Form["koiFishId"]);
            koiFish.Name = Request.Form["koiName"];
            if (koiImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    koiImage.CopyTo(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }

            koiFish.ImageData = imageBytes;
            koiFish.Origin = Request.Form["koiOrigin"];
            koiFish.Gender = Request.Form["koiGender"];
            try
            {
                koiFish.Age = int.Parse(Request.Form["koiAge"]);
            }
            catch (Exception ex)
            {
                TempData["KoiFishFail"] = "Tuổi cá Koi phải là số nguyên.";
            }

            try
            {
                koiFish.Size = int.Parse(Request.Form["koiSize"]);
            }
            catch (Exception ex)
            {
                TempData["KoiFishFail"] = "Kích thước cá Koi phải là số nguyên.";
            }

            koiFish.Breed = Request.Form["koiBreed"];
            try
            {
                koiFish.FilterRatio = double.Parse(Request.Form["filterRatio"]);
            }
            catch (Exception ex)
            {
                TempData["KoiFishFail"] = "Tỷ lệ lọc cá Koi phải là số.";
            }

            try
            {
                koiFish.Price = double.Parse(Request.Form["koiPrice"]);
            }
            catch (Exception ex)
            {
                TempData["KoiFishFail"] = "Giá cá Koi phải là số.";
            }
            
            koiFish.UpdateAt = DateTime.Now;
            koiFish.IsDeleted = false;
            koiFish.Color = Request.Form["koiColor"];
            if (_koiFishRepository.UpdateKoiFish(koiFish))
            {
                TempData["KoiFishSuccess"] = "Cập nhật cá Koi thành công";

                return RedirectToPage("/Staff/KoiFishManagement");
            }
            else
            {
                Message = "Cập nhật cá Koi thất bại";
                return Page();
            }
        }

        Koifish = _koiFishRepository.GetKoiFishByIdByStaff(long.Parse(Request.Form["koiFishId"]));
        return Page();
    }
}