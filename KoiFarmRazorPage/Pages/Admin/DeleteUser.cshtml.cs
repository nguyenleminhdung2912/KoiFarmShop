using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccessObject;
using Microsoft.AspNetCore.SignalR;
using NguyenLeMinhDungFall2024RazorPages;
using NuGet.Protocol.Plugins;
using Repository.IRepository;
using Repository.Repository;

namespace KoiFarmRazorPage.Pages.Admin
{
    public class DeleteModel : PageModel
    {
        private readonly IUserRepository userRepository;
        private readonly IHubContext<SignalRHub> hubContext;

        public DeleteModel(IHubContext<SignalRHub> hubContext)
        {
            userRepository = new UserRepository();
            this.hubContext = hubContext;

        }

        [BindProperty] public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = userRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userRepository.GetUserByIdToDelete(id);
            Wallet wallet = user.Wallets.FirstOrDefault();
            if (wallet != null)
            {
                if (wallet.Total != 0)
                {
                    TempData["DeleteMessage"] = "Cannot delete this User because they have money in their wallet.";
                    return RedirectToPage("/Admin/DeleteUser", new { id = user.UserId });
                }
            }

            List<Order> orders = user.Orders.ToList();
            if (orders.Count > 0)
            {
                foreach (var order in orders)
                {
                    if (order.Status.Equals("PAID") && order.ShipmentStatus.Equals("PREPARING") ||
                        order.Status.Equals("PAID") && order.ShipmentStatus.Equals("ONGOING") ||
                        order.Status.Equals("PAID") && order.ShipmentStatus.Equals("NOTYET") )
                    {
                        TempData["DeleteMessage"] = "Cannot delete this User because they have going on order.";
                        return RedirectToPage("/Admin/DeleteUser", new { id = user.UserId });
                    }
                }
            }

            List<Consignment> consignments = user.Consignments.ToList();
            if (consignments.Count > 0)
            {
                foreach (var consignment in consignments)
                {
                    if (consignment.Status.Equals("PENDING") ||
                        consignment.Status.Equals("APPROVED") ||
                        consignment.Status.Equals("CONFIRMED")
                       )
                    {
                        TempData["DeleteMessage"] = "Cannot delete this User because they have going on consignment.";
                        return RedirectToPage("/Admin/DeleteUser", new { id = user.UserId });
                    }
                }
            }

            user.IsDeleted = true;
            userRepository.UpdateUser(user);
            TempData["DeleteMessage"] = "Delete user successfully.";
            
            await hubContext.Clients.All.SendAsync("RefreshData");
            
            return RedirectToPage("/Admin/Index");
        }
    }
}