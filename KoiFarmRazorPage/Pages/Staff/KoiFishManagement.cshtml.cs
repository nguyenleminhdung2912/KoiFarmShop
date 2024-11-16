using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using NguyenLeMinhDungFall2024RazorPages;
using Repository.IRepository;

namespace KoiFarmRazorPage.Pages.Staff;
[Authorize(Roles = "Staff")]

public class KoiFishManagement : PageModel
{
    private readonly IKoiFishRepository _repository;
    private readonly IHubContext<SignalRHub> hubContext;


    public string Message { get; set; }

    public List<KoiFish> KoiFishes { get; set; } = new List<KoiFish>();

    public KoiFishManagement(IKoiFishRepository repository, IHubContext<SignalRHub> hubContext)
    {
        this._repository = repository;
        this.hubContext = hubContext;
    }

    public void OnGet()
    {
        KoiFishes = _repository.GetkoiFishes();
    }

    public IActionResult OnPost()
    {
        string handler = Request.Form["handler"];
        if (handler == "Search")
        {
            if (string.IsNullOrEmpty(Request.Form["koiName"]))
            {
                TempData["SearchFail"] = "Không có cá koi nào tồn tại với tên này!!!";
                KoiFishes = _repository.GetkoiFishes();
            }
            else
            {
                if (_repository.GetKoiFishByName(Request.Form["koiName"]).IsNullOrEmpty())
                {
                    TempData["SearchFail"] = "Không có cá koi nào tồn tại với tên này!!!";
                    KoiFishes = _repository.GetKoiFishByName(Request.Form["koiName"]);
                }
                else
                {
                    KoiFishes = _repository.GetKoiFishByName(Request.Form["koiName"]);
                }
            }
        }
        if (handler == "Create")
        {
            return RedirectToPage("/Staff/CreateKoiFish");
        }

        if (handler == "Delete")
        {
            if (string.IsNullOrEmpty(Request.Form["selectedKoiFishId"]))
            {
                Message = "Chọn một cá koi cụ thể để xoá";
                KoiFishes = _repository.GetkoiFishes();
            }
            else
            {
                long koiFishId = long.Parse(Request.Form["selectedKoiFishId"]);
                if (_repository.DeleteKoiFishById(koiFishId))
                {
                    TempData["KoiFishSuccess"] = "Xoá cá koi thành công";
                    KoiFishes = _repository.GetkoiFishes();
                    return Page();
                }
                else
                {
                    Message = "Xoá cá koi thất bại";
                    KoiFishes = _repository.GetkoiFishes();
                    return Page();
                }
            }
        }

        if (handler == "Update")
        {
            if (string.IsNullOrEmpty(Request.Form["selectedKoiFishId"]))
            {
                Message = "Chọn một cá koi cụ thể để cập nhật ";
                KoiFishes = _repository.GetkoiFishes();
            }
            else
            {
               return RedirectToPage("/Staff/UpdateKoiFish", new { id = long.Parse(Request.Form["selectedKoiFishId"]) });
            }
        }
        // hubContext.Clients.All.SendAsync("RefreshData");

        return Page();
    }
}