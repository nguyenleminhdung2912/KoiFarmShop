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

        differences.Add("Name", (koiFish1.Name, koiFish2.Name));
        differences.Add("Origin", (koiFish1.Origin, koiFish2.Origin));
        differences.Add("Gender", (koiFish1.Gender, koiFish2.Gender));
        differences.Add("Color", (koiFish1.Color, koiFish2.Color));
        differences.Add("Age", (koiFish1.Age, koiFish2.Age));
        differences.Add("Size", (koiFish1.Size, koiFish2.Size));
        differences.Add("Breed", (koiFish1.Breed, koiFish2.Breed));
        differences.Add("Quantity", (koiFish1.Quantity, koiFish2.Quantity));
        differences.Add("FilterRatio", (koiFish1.FilterRatio, koiFish2.FilterRatio));
        differences.Add("Price", (koiFish1.Price, koiFish2.Price));
        differences.Add("Status", (koiFish1.Status, koiFish2.Status));
        differences.Add("CreateAt", (koiFish1.CreateAt, koiFish2.CreateAt));
        differences.Add("UpdateAt", (koiFish1.UpdateAt, koiFish2.UpdateAt));
        differences.Add("IsDeleted", (koiFish1.IsDeleted, koiFish2.IsDeleted));

        return differences;
    }
}