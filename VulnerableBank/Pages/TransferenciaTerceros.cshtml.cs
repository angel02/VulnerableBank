using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VulnerableBank.Data;
using VulnerableBank.Data.Models;

namespace VulnerableBank.Pages
{
    [Authorize]
    public class TransferenciaTerceroModel(ILogger<TransferenciaTerceroModel> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context) : PageModel
    {

        public class InputModel
        {
            public int AccountSource { get; set; }
            public int AccountDestination { get; set; }
            public decimal Amount { get; set; }
            public string Description { get; set; } = String.Empty;
        }

        public SelectList UserAccounts { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Input = new InputModel();

            var user = await GetUser();

            var userAccounts = await context.Accounts.Where(x => x.UserId == user.Id).AsNoTracking().ToListAsync();
                userAccounts.ForEach(x => x.Alias += $" (balance RD$ {x.Balance:n})");

            UserAccounts = new SelectList(userAccounts, "Number", "Alias");

            return Page();
        }


        public async Task<IActionResult> OnPost()
        {
            var user = await GetUser();
            var accountSource = await context.Accounts.Where(x => x.UserId == user.Id)
                                           .FirstOrDefaultAsync(x => x.Number == Input.AccountSource);
            

            if(accountSource == null) {
                TempData["ValidacionTransferencia"] = "La cuenta no existe";
                return RedirectToPage();
            }


            if(accountSource.Balance < Input.Amount) {
                TempData["ValidacionTransferencia"] = "No cuenta con balance suficiente";
                return RedirectToPage();
            }


            accountSource.Balance -= Input.Amount;

            var accountDestination = await context.Accounts.FirstOrDefaultAsync(x => x.Number == Input.AccountDestination);
            if(accountDestination == null) {
                TempData["ValidacionTransferencia"] = "La cuenta a la que intenta transferir no existe";
                return RedirectToPage();
            }

            accountDestination.Balance += Input.Amount;

            await context.SaveChangesAsync();
            TempData["TransferenciaExitosa"] = "La transacción se realizó exitosamente";

            return RedirectToPage("Index");
        }


        private async Task<ApplicationUser> GetUser()
        {
            return await userManager.GetUserAsync(User);
        }
    }

}
