using Service.Models;

namespace KoiFarmRazorPage.Service;

public interface IVnPayService
{
    string CreatePaymentUrl(HttpContext context, double amount);

    VnPaymentResponseModel MakePayment(IQueryCollection colletions);
}