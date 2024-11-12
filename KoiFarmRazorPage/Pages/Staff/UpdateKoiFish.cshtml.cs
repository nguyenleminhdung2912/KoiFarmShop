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
            ValidateErrors["KoiName"] = "KoiName is required";
        }
        // else if (koiImage == null)
        // {
        //     ValidateErrors["KoiImage"] = "KoiImage is required";
        // }
        else if (string.IsNullOrEmpty(Request.Form["koiOrigin"]))
        {
            ValidateErrors["KoiOrigin"] = "Origin is required";
        }
        else if (string.IsNullOrEmpty(Request.Form["koiGender"]))
        {
            ValidateErrors["KoiGender"] = "Koi Gender is required";
        }
        else if (string.IsNullOrEmpty(Request.Form["koiAge"]))
        {
            ValidateErrors["KoiAge"] = "Koi Age is required";
        }
        else if (string.IsNullOrEmpty(Request.Form["koiSize"]))
        {
            ValidateErrors["KoiSize"] = "Koi Size is required";
        }
        else if (string.IsNullOrEmpty(Request.Form["koiBreed"]))
        {
            ValidateErrors["Breed"] = "Koi Breed is required";
        }
        else if (string.IsNullOrEmpty(Request.Form["filterRatio"]))
        {
            ValidateErrors["FilterRatio"] = "Filter Ratio is required";
        }
        else if (string.IsNullOrEmpty(Request.Form["koiPrice"]))
        {
            ValidateErrors["KoiPrice"] = "Koi Price is required";
        }
        // else if (string.IsNullOrEmpty(Request.Form["koiStatus"]))
        // {
        //     ValidateErrors["KoiStatus"] = "Status is required";
        // }
        else if (string.IsNullOrEmpty(Request.Form["koiColor"]))
        {
            ValidateErrors["KoiColor"] = "Color is required";
        }
        else if (string.IsNullOrEmpty(Request.Form["koiQuantity"]))
        {
            ValidateErrors["KoiQuantity"] = "Quantity is required";
        }
        else if (int.Parse(Request.Form["koiAge"]) <= 0)
        {
            ValidateErrors["KoiAge"] = "KoiFish Age must be > 0";
        }
        else if (int.Parse(Request.Form["koiSize"]) <= 0)
        {
            ValidateErrors["KoiSize"] = "KoiFish Size must be > 0";
        }
        else if (int.Parse(Request.Form["koiQuantity"]) < 0)
        {
            ValidateErrors["KoiQuantity"] = "KoiFish Quantity must be >= 0";
        }
        else if (double.Parse(Request.Form["koiPrice"]) <= 0)
        {
            ValidateErrors["KoiPrice"] = "KoiFish Price must be > 0";
        }
        else if (double.Parse(Request.Form["filterRatio"]) <= 0)
        {
            ValidateErrors["FilterRatio"] = "KoiFish Filter Ratio Must be > 0";
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
                TempData["KoiFishFail"] = "KoiFish Age must be an integer number.";
            }

            try
            {
                koiFish.Size = int.Parse(Request.Form["koiSize"]);
            }
            catch (Exception ex)
            {
                TempData["KoiFishFail"] = "Koi Size Age must be an integer number.";
            }

            koiFish.Breed = Request.Form["koiBreed"];
            try
            {
                koiFish.FilterRatio = double.Parse(Request.Form["filterRatio"]);
            }
            catch (Exception ex)
            {
                TempData["KoiFishFail"] = "KoiFish Filter Ratio Must be a number.";
            }

            try
            {
                koiFish.Price = double.Parse(Request.Form["koiPrice"]);
            }
            catch (Exception ex)
            {
                TempData["KoiFishFail"] = "KoiFish price Must be a number.";
            }

            if (koiFish.Quantity == 0)
            {
                koiFish.Status = "Out of Stock";
            }

            koiFish.Status = "Available";
            koiFish.UpdateAt = DateTime.Now;
            koiFish.IsDeleted = false;
            koiFish.Color = Request.Form["koiColor"];
            try
            {
                koiFish.Quantity = int.Parse(Request.Form["koiQuantity"]);
            }
            catch (Exception ex)
            {
                TempData["KoiFishFail"] = "KoiFish Quantity must be an integer number.";
            }

            if (_koiFishRepository.UpdateKoiFish(koiFish))
            {
                TempData["KoiFishSuccess"] = "Update Koi Fish sucessfully";

                return RedirectToPage("/Staff/KoiFishManagement");
            }
            else
            {
                Message = "Update Koi Fish Failed";
                return Page();
            }
        }

        Koifish = _koiFishRepository.GetKoiFishByIdByStaff(long.Parse(Request.Form["koiFishId"]));
        return Page();
    }
}