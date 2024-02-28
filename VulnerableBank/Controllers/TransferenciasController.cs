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

        /// <summary>
        /// Retorna un arreglo con el mensaje y cambia el Status Code
        /// </summary>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        private List<string> RespuestaNoValida(string mensaje)
        {
            Response.StatusCode = 400;
            return new List<string> { mensaje };
        }


        [HttpPost("[action]")]
        public async Task<List<string>> Terceros([FromBody]TransferRequest request)
        {
            var respuestas = new List<string>();
            var user = await userManager.GetUserAsync(User);
            var userAccounts = await context.Accounts.Where(x => x.UserId == user.Id).ToListAsync();
            var accountSource = userAccounts.FirstOrDefault(x => x.Number == request.AccountSource);


            if (accountSource == null) {
                return RespuestaNoValida("La cuenta no existe");
            }

            if (accountSource.Balance < request.Amount) {
                return RespuestaNoValida("No cuenta con balance suficiente");
            }


            var accountDestination = userAccounts.FirstOrDefault(x => x.Number == request.AccountDestination);
            if (accountDestination == null) {
                return RespuestaNoValida("La cuenta a la que intenta transferir no existe");
            }


            if (accountSource.Number == accountDestination.Number) {
                return RespuestaNoValida("No puede transferir dinero a la misma cuenta");
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
                return RespuestaNoValida("La cuenta de origen no existe");
            }

            if (accountSource.Balance < request.Amount) {
                return RespuestaNoValida("No cuenta con balance suficiente");
            }


            var accountDestination = userAccounts.FirstOrDefault(x => x.Number == request.AccountDestination);
            if (accountDestination == null) {
                return RespuestaNoValida("La cuenta destinataria no existe");
            }


            if(accountSource.Number == accountDestination.Number) {
                return RespuestaNoValida("No puede transferir dinero a la misma cuenta");
            }


            accountSource.Balance -= request.Amount;
            accountDestination.Balance += request.Amount;

            await context.SaveChangesAsync();

            return respuestas;
        }
    }
}
