using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VulnerableBank.Data.Models;
using VulnerableBank.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace VulnerableBank.Pages
{
    [Authorize(Roles = "customer")]
    public class TransferenciaPropiaModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context) : PageModel
    {
        public SelectList AccountOrigins { get; set; }
        public SelectList AccountDestinations { get; set; }


        public async Task<IActionResult> OnGet()
        {
            var user = await userManager.GetUserAsync(User);
            var userAccounts = await context.Accounts.Where(x => x.UserId == user.Id).AsNoTracking().ToListAsync();
                userAccounts.ForEach(x => x.Alias += $" (balance RD$ {x.Balance:n})");

            AccountOrigins = new SelectList(userAccounts, "Number", "Alias");
            AccountDestinations = new SelectList(userAccounts, "Number", "Alias");

            return Page();
        }
    }
}
