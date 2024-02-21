using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VulnerableBank.Data;
using VulnerableBank.Data.Dtos;
using VulnerableBank.Data.Models;

namespace VulnerableBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferenciasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        [HttpPost("[action]")]
        public async Task<List<string>> Terceros([FromBody]TransferRequest request)
        {
            var respuestas = new List<string>();
            var user = await userManager.GetUserAsync(User);
            var accountSource = await context.Accounts.Where(x => x.UserId == user.Id)
                                           .FirstOrDefaultAsync(x => x.Number == request.AccountSource);


            if (accountSource == null) {
                respuestas.Add("La cuenta no existe");
                return respuestas;
            }

            if (accountSource.Balance < request.Amount) {
                respuestas.Add("No cuenta con balance suficiente");
                return respuestas;
            }


            var accountDestination = await context.Accounts.FirstOrDefaultAsync(x => x.Number == request.AccountDestination);
            if (accountDestination == null) {
                respuestas.Add("La cuenta a la que intenta transferir no existe");
                return respuestas;
            }


            if (accountSource.Number == accountDestination.Number) {
                respuestas.Add("No puede transferir dinero a la misma cuenta");
                return respuestas;
            }


            accountSource.Balance -= request.Amount;
            accountDestination.Balance += request.Amount;

            await context.SaveChangesAsync();

            return respuestas;
        }

        [HttpPost("[action]")]
        public async Task<List<string>> Propias([FromBody]TransferRequest request)
        {
            var respuestas = new List<string>();
            var user = await userManager.GetUserAsync(User);
            var userAccounts = await context.Accounts.Where(x => x.UserId == user.Id).ToListAsync();

            var accountSource = userAccounts.FirstOrDefault(x => x.Number == request.AccountSource);
            if (accountSource == null) {
                respuestas.Add("La cuenta de origen no existe");
                return respuestas;
            }

            if (accountSource.Balance < request.Amount) {
                respuestas.Add("No cuenta con balance suficiente");
                return respuestas;
            }


            var accountDestination = userAccounts.FirstOrDefault(x => x.Number == request.AccountDestination);
            if (accountDestination == null) {
                respuestas.Add("La cuenta destinataria no existe");
                return respuestas;
            }


            if(accountSource.Number == accountDestination.Number) {
                respuestas.Add("No puede transferir dinero a la misma cuenta");
                return respuestas;
            }


            accountSource.Balance -= request.Amount;
            accountDestination.Balance += request.Amount;

            await context.SaveChangesAsync();

            return respuestas;
        }
    }
}
