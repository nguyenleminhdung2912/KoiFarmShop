using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;
using Repository.Repository;

namespace KoiFarmRazorPage.Pages.Customer;

public class CompareKoiFishes : PageModel
{
    private readonly IKoiFishRepository koiFishRepository;

    public KoiFish KoiFish1 { get; set; }
    public KoiFish KoiFish2 { get; set; }
    public Dictionary<string, (object? KoiFish1Value, object? KoiFish2Value)> Differences { get; set; }

    public CompareKoiFishes ()
    {
        koiFishRepository = new KoiFishRepository();
    }

    public async Task<IActionResult> OnGetAsync(long[] selectedKoiFishIds)
    {
        if (selectedKoiFishIds.Length != 2)
        {
            return RedirectToPage("/Customer/ViewKoiFish");
        }

        KoiFish1 = await koiFishRepository.GetKoiFishById(selectedKoiFishIds[0]);
        KoiFish2 = await koiFishRepository.GetKoiFishById(selectedKoiFishIds[1]);

        if (KoiFish1 == null || KoiFish2 == null)
        {
            return NotFound();
        }

        Differences = CompareKoiFishesAttributes(KoiFish1, KoiFish2);

        return Page();
    }
    
    private Dictionary<string, (object? KoiFish1Value, object? KoiFish2Value)> CompareKoiFishesAttributes(KoiFish koiFish1, KoiFish koiFish2)
    {
        var differences = new Dictionary<string, (object? KoiFish1Value, object? KoiFish2Value)>();

        differences.Add("Tên", (koiFish1.Name, koiFish2.Name));
        differences.Add("Nguồn gốc", (koiFish1.Origin, koiFish2.Origin));
        differences.Add("Giới tính", (koiFish1.Gender.Equals("Female") ? "Cái" : "Đực", koiFish1.Gender.Equals("Female") ? "Cái" : "Đực"));
        differences.Add("Màu sắc", (koiFish1.Color, koiFish2.Color));
        differences.Add("Tuổi", (koiFish1.Age, koiFish2.Age));
        differences.Add("Kích thước", (koiFish1.Size, koiFish2.Size));
        differences.Add("Giống loài", (koiFish1.Breed, koiFish2.Breed));
        differences.Add("Tỉ lệ sàng lọc", (koiFish1.FilterRatio, koiFish2.FilterRatio));
        differences.Add("Giá", (koiFish1.Price, koiFish2.Price));
        differences.Add("Trạng thái", (koiFish1.Status, koiFish2.Status));

        return differences;
    }
}