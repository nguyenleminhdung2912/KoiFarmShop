using BusinessObject;
using KoiFarmRazorPage.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;
using Repository.Repository;
using Service.Models;

namespace KoiFarmRazorPage.Pages.Customer;

[Authorize(Roles = "Customer")]

public class PaymentResult : PageModel
{
    private readonly IVnPayService _vnPayService;
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletLogRepository _walletLogRepository;
    private readonly IUserRepository _userRepository;

    public PaymentResult(IVnPayService vnPayService, IUserRepository userRepository)
    {
        _vnPayService = vnPayService;
        _walletRepository = new WalletRepository();
        _walletLogRepository = new WalletLogRepository();
        _userRepository = userRepository;
    }

    public VnPaymentResponseModel PaymentResponse { get; set; }

    public async void OnGet()
    {
        var response = _vnPayService.MakePayment(Request.Query);

        if (response != null)
        {
            PaymentResponse = response;
            PaymentResponse.VnPayAmount = response.VnPayAmount / 100;
            //Check if this payment already exist
            WalletLog? walletLog = _walletLogRepository.GetWalletLogByType("Deposit " + PaymentResponse.OrderId);
            if (walletLog == null)
            {
                //Add money to wallet
                var email = User.Identity.Name;
            
                Wallet wallet = await _walletRepository.GetWalletByUserEmail(email);
                wallet.Total += PaymentResponse.VnPayAmount;
                _walletRepository.Update(wallet);

                //Create wallet log
                walletLog = new WalletLog
                {
                    WalletId = wallet.WalletId,
                    Amount = PaymentResponse.VnPayAmount,
                    Type = "Deposit " + PaymentResponse.OrderId,
                    CreateAt = DateTime.Now,
                    IsDeleted = false
                };
                _walletLogRepository.CreateWalletLog(walletLog);
            }
        }
    }
}