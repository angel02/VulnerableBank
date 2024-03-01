using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VulnerableBank.Data;
using VulnerableBank.Data.Dtos;
using VulnerableBank.Data.Models;

namespace VulnerableBank.Controllers
{
    [Authorize]
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
            Response.StatusCode = 200;
            return new List<string> { mensaje };
        }


        [HttpPost("[action]")]
        public async Task<List<string>> Terceros([FromBody]TransferRequest request)
        {
            var respuestas = new List<string>();
            var user = await userManager.GetUserAsync(User);

            // Se obtienen las cuentas del usuario
            var userAccounts = await context.Accounts.Where(x => x.UserId == user.Id).ToListAsync();

            // Se valida que la cuenta de origen pertenezca realmente al usuario
            var accountSource = userAccounts.FirstOrDefault(x => x.Number == request.AccountSource);
            if (accountSource == null) {
                return RespuestaNoValida("La cuenta no existe");
            }

            // Se confirma balance disponible
            if (accountSource.Balance < request.Amount) {
                return RespuestaNoValida("No cuenta con balance suficiente");
            }

            // Se valida existencia de cuenta destinataria
            var accountDestination = await context.Accounts.FirstOrDefaultAsync(x => x.Number == request.AccountDestination);
            if (accountDestination == null) {
                return RespuestaNoValida("La cuenta a la que intenta transferir no existe");
            }

            // Se valida si ambas cuentas son la misma
            if (accountSource.Number == accountDestination.Number) {
                return RespuestaNoValida("No puede transferir dinero a la misma cuenta");
            }

            // Se realiza la operación
            accountSource.Balance -= request.Amount;
            accountDestination.Balance += request.Amount;

            // Se registra la transacción
            context.Transactions.Add(new Transaction() {
                SourceAccountNumber = accountSource.Number,
                DestinationAccountNumber = accountDestination.Number,
                Amount = request.Amount,
                Description = request.Description,
                Date = DateTime.Now
            });

            // Se aplican los cambios a la base de datos
            await context.SaveChangesAsync();

            return respuestas;
        }



        [HttpPost("[action]")]
        public async Task<List<string>> Propias([FromBody]TransferRequest request)
        {
            var respuestas = new List<string>();

            // Se obtienen las cuentas del usuario
            var user = await userManager.GetUserAsync(User);
            var userAccounts = await context.Accounts.Where(x => x.UserId == user.Id).ToListAsync();

            // Se confirma que la cuenta de origen pertenezca al usuario
            var accountSource = userAccounts.FirstOrDefault(x => x.Number == request.AccountSource);
            if (accountSource == null) {
                return RespuestaNoValida("La cuenta de origen no existe");
            }


            // Se valida el balance disponible
            if (accountSource.Balance < request.Amount) {
                return RespuestaNoValida("No cuenta con balance suficiente");
            }

            // Se valida que la cuenta destino sea del mismo usuario
            var accountDestination = userAccounts.FirstOrDefault(x => x.Number == request.AccountDestination);
            if (accountDestination == null) {
                return RespuestaNoValida("La cuenta destinataria no existe");
            }

            // Se valida que no sea la misma cuenta
            if(accountSource.Number == accountDestination.Number) {
                return RespuestaNoValida("No puede transferir dinero a la misma cuenta");
            }

            // Se realiza la operaci[on
            accountSource.Balance -= request.Amount;
            accountDestination.Balance += request.Amount;

            // Se registra la transacción
            context.Transactions.Add(new Transaction() {
                SourceAccountNumber = accountSource.Number,
                DestinationAccountNumber = accountDestination.Number,
                Amount = request.Amount,
                Description = request.Description,
                Date = DateTime.Now
            });


            // Se guardan los cambios
            await context.SaveChangesAsync();

            return respuestas;
        }
    }
}
