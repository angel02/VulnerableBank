using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VulnerableBank.Data;
using VulnerableBank.Data.Models;

namespace VulnerableBank.Pages
{
    [Authorize]
    public class IndexModel(ILogger<IndexModel> 
        logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager) : PageModel
    {
        public List<Account> UserAccounts = new List<Account>();
        public List<Transaction> Transactions = new List<Transaction>();
        public async Task<IActionResult> OnGet()
        {
            var user = await userManager.GetUserAsync(User);
            UserAccounts = await context.Accounts.Where(x => x.UserId == user.Id).ToListAsync();
            var accountIdArray = UserAccounts.Select(x => x.Number).ToArray();
            Transactions = await context.Transactions
                .Where(x => accountIdArray.Contains(x.SourceAccountNumber) 
                         || accountIdArray.Contains(x.DestinationAccountNumber)
                ).AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

            return Page();
        }
    }
}
