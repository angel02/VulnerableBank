using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VulnerableBank.Data;
using VulnerableBank.Data.Models;

namespace VulnerableBank.Pages
{
    [Authorize(Roles = "customer")]
    public class IndexModel(ILogger<IndexModel> 
        logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager) : PageModel
    {
        public List<Account> UserAccounts = new List<Account>();
        public List<Transaction> Transactions = new List<Transaction>();
        public ApplicationUser CustomerUser { get; set; }


        public async Task<IActionResult> OnGet()
        {
            // Get user instance
            CustomerUser = await userManager.GetUserAsync(User);

            // List user accounts
            UserAccounts = await context.Accounts.Where(x => x.UserId == CustomerUser.Id).ToListAsync();

            // Get all transactions related to user's accounts
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
