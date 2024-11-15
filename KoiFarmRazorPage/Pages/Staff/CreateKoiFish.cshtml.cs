using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff;

[Authorize(Roles = "Staff")]

public class CreateKoiFish : PageModel
{
    private readonly IKoiFishRepository _koiFishRepository;
    private readonly IHubContext<SignalRHub> hubContext;


    public Dictionary<string, string> ValidateErrors { get; set; } = new Dictionary<string, string>();
    public string Message { get; set; }

    public CreateKoiFish(IKoiFishRepository koiFishRepository, IHubContext<SignalRHub> hubContext)
    {
        this._koiFishRepository = koiFishRepository;
        this.hubContext = hubContext;
    }
    public void OnGet()
    {
        
    }

    public IActionResult OnPost()
    {
        var koiImage = Request.Form.Files["koiImage"];
        if (string.IsNullOrEmpty(Request.Form["koiName"]))
        {
            ValidateErrors["KoiName"] = "Tên cá koi không được để trống";
        }else if (koiImage == null)
        {
            ValidateErrors["KoiImage"] = "Hình ảnh không được để trống";
        }else if (string.IsNullOrEmpty(Request.Form["koiOrigin"]))
        {
            ValidateErrors["KoiOrigin"] = "Nguồn gốc cá koi không được để trống";
        }else if (string.IsNullOrEmpty(Request.Form["koiGender"]))
        {
            ValidateErrors["KoiGender"] = "Giới tinh cá koi không được để trống";
        }else if (string.IsNullOrEmpty(Request.Form["koiAge"]))
        {
            ValidateErrors["KoiAge"] = "Tuổi cá koi không được để trống";
        }else if (string.IsNullOrEmpty(Request.Form["koiSize"]))
        {
            ValidateErrors["KoiSize"] = "Kích thước cá koi không được để trống";
        }else if (string.IsNullOrEmpty(Request.Form["koiBreed"]))
        {
            ValidateErrors["Breed"] = "Giống loài cá koi không được để trống";
        }else if (string.IsNullOrEmpty(Request.Form["filterRatio"]))
        {
            ValidateErrors["FilterRatio"] = "Tỉ lệ sàn lọc cá koi không được để trống";
        }else if (string.IsNullOrEmpty(Request.Form["koiPrice"]))
        {
            ValidateErrors["KoiPrice"] = "Giá cá koi không được để trống";
        }
        // else if (string.IsNullOrEmpty(Request.Form["koiStatus"]))
        // {
        //     ValidateErrors["KoiStatus"] = "Status is required";
        // }
        else if (string.IsNullOrEmpty(Request.Form["koiColor"]))
        {
            ValidateErrors["KoiColor"] = "Màu cá koi không được để trống";
        }
        // else if (string.IsNullOrEmpty(Request.Form["koiQuantity"]))
        // {
        //     ValidateErrors["KoiQuantity"] = "Quantity is required";
        // }
        else if (int.Parse(Request.Form["koiAge"]) <= 0)
        {
            ValidateErrors["KoiAge"] = "Tuổi cá koi phải lớn hơn 0";
        }else if (int.Parse(Request.Form["koiSize"]) <= 0)
        {
            ValidateErrors["KoiSize"] = "Kích thước cá koi phải lớn hơn 0";
        }
        // else if (int.Parse(Request.Form["koiQuantity"]) <= 0)
        // {
        //     ValidateErrors["KoiQuantity"] = "KoiFish Quantity must be > 0";
        // }
        else if (double.Parse(Request.Form["koiPrice"]) <= 0)
        {
            ValidateErrors["KoiPrice"] = "Kích thước cá koi lớn hơn  0";
        }else if (double.Parse(Request.Form["filterRatio"]) <= 0)
        {
            ValidateErrors["FilterRatio"] = "Tỷ lẹ sàn lọc cá koi lớn hơn  0";
        }
        else
        {
            byte[] imageBytes = null;
            KoiFish koiFish = new KoiFish();
            koiFish.KoiFishId = GetId();
            koiFish.Name = Request.Form["koiName"];
            using (var memoryStream = new MemoryStream())
            {
                koiImage.CopyTo(memoryStream);
                imageBytes = memoryStream.ToArray();
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
                ValidateErrors["KoiAge"] = "Tuổi cá koi phải là số nguyên";
            }
            
            try
            {
                koiFish.Size = int.Parse(Request.Form["koiSize"]);
            }
            catch (Exception ex)
            {
                ValidateErrors["KoiSize"] = "Kích thước cá koi phải là số nguyên";
            }
            koiFish.Breed = Request.Form["koiBreed"];
            try
            {
                koiFish.FilterRatio = double.Parse(Request.Form["filterRatio"]);
            }
            catch (Exception ex)
            {
                ValidateErrors["FilterRatio"] = "Tỷ lệ sàn lọc cá koi phải là số nguyên";
            }
            
            try
            {
                koiFish.Price= double.Parse(Request.Form["koiPrice"]);
            }
            catch (Exception ex)
            {
                ValidateErrors["KoiPrice"] = "Giá cá koi phải là số nguyên.";
            }
            
            koiFish.Status = "Available";
            koiFish.CreateAt = DateTime.Now;
            koiFish.IsDeleted = false;
            koiFish.Color = Request.Form["koiColor"];
            koiFish.Quantity = 1;
            // try
            // {
            //     koiFish.Quantity = int.Parse(Request.Form["koiQuantity"]);
            // }
            // catch (Exception ex)
            // {
            //     ValidateErrors["KoiQuantity"] = "KoiFish Quantity must be an integer number.";
            // }

            if (_koiFishRepository.CreateKoiFish(koiFish))
            {
                TempData["KoiFishSuccess"] = "Tạo cá koi thành công";
                hubContext.Clients.All.SendAsync("RefreshData");

                return RedirectToPage("/Staff/KoiFishManagement");
            }
            else
            {
                Message = "Tạo cá koi không thành công";
                return Page();
            }
        }

        return Page();
    }

    private long GetId()
    {
       return  _koiFishRepository.GetKoiFishId();
    }
}