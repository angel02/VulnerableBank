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
        public async Task<IActionResult> OnGet()
        {
            var user = await userManager.GetUserAsync(User);
            UserAccounts = await context.Accounts.Where(x => x.UserId == user.Id).ToListAsync();

            return Page();
        }
    }
}
