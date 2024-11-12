using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff;

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
            ValidateErrors["KoiName"] = "KoiName is required";
        }else if (koiImage == null)
        {
            ValidateErrors["KoiImage"] = "KoiImage is required";
        }else if (string.IsNullOrEmpty(Request.Form["koiOrigin"]))
        {
            ValidateErrors["KoiOrigin"] = "Origin is required";
        }else if (string.IsNullOrEmpty(Request.Form["koiGender"]))
        {
            ValidateErrors["KoiGender"] = "Koi Gender is required";
        }else if (string.IsNullOrEmpty(Request.Form["koiAge"]))
        {
            ValidateErrors["KoiAge"] = "Koi Age is required";
        }else if (string.IsNullOrEmpty(Request.Form["koiSize"]))
        {
            ValidateErrors["KoiSize"] = "Koi Size is required";
        }else if (string.IsNullOrEmpty(Request.Form["koiBreed"]))
        {
            ValidateErrors["Breed"] = "Koi Breed is required";
        }else if (string.IsNullOrEmpty(Request.Form["filterRatio"]))
        {
            ValidateErrors["FilterRatio"] = "Filter Ratio is required";
        }else if (string.IsNullOrEmpty(Request.Form["koiPrice"]))
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
        }else if (string.IsNullOrEmpty(Request.Form["koiQuantity"]))
        {
            ValidateErrors["KoiQuantity"] = "Quantity is required";
        }else if (int.Parse(Request.Form["koiAge"]) <= 0)
        {
            ValidateErrors["KoiAge"] = "KoiFish Age must be > 0";
        }else if (int.Parse(Request.Form["koiSize"]) <= 0)
        {
            ValidateErrors["KoiSize"] = "KoiFish Size must be > 0";
        }else if (int.Parse(Request.Form["koiQuantity"]) <= 0)
        {
            ValidateErrors["KoiQuantity"] = "KoiFish Quantity must be > 0";
        }else if (double.Parse(Request.Form["koiPrice"]) <= 0)
        {
            ValidateErrors["KoiPrice"] = "KoiFish Price must be > 0";
        }else if (double.Parse(Request.Form["filterRatio"]) <= 0)
        {
            ValidateErrors["FilterRatio"] = "KoiFish Filter Ratio Must be > 0";
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
                ValidateErrors["KoiAge"] = "KoiFish Age must be an integer number.";
            }
            
            try
            {
                koiFish.Size = int.Parse(Request.Form["koiSize"]);
            }
            catch (Exception ex)
            {
                ValidateErrors["KoiSize"] = "Koi Size Age must be an integer number.";
            }
            koiFish.Breed = Request.Form["koiBreed"];
            try
            {
                koiFish.FilterRatio = double.Parse(Request.Form["filterRatio"]);
            }
            catch (Exception ex)
            {
                ValidateErrors["FilterRatio"] = "KoiFish Filter Ratio Must be a number.";
            }
            
            try
            {
                koiFish.Price= double.Parse(Request.Form["koiPrice"]);
            }
            catch (Exception ex)
            {
                ValidateErrors["KoiPrice"] = "KoiFish price Must be a number.";
            }
            
            koiFish.Status = "Available";
            koiFish.CreateAt = DateTime.Now;
            koiFish.IsDeleted = false;
            koiFish.Color = Request.Form["koiColor"];
            try
            {
                koiFish.Quantity = int.Parse(Request.Form["koiQuantity"]);
            }
            catch (Exception ex)
            {
                ValidateErrors["KoiQuantity"] = "KoiFish Quantity must be an integer number.";
            }

            if (_koiFishRepository.CreateKoiFish(koiFish))
            {
                TempData["KoiFishSuccess"] = "Create Koi Fish sucessfully";
                hubContext.Clients.All.SendAsync("RefreshData");

                return RedirectToPage("/Staff/KoiFishManagement");
            }
            else
            {
                Message = "Create Koi Fish Failed";
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