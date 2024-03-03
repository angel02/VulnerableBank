using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VulnerableBank.Data.Models;

namespace VulnerableBank.Data.Seeder
{
    public static class MasterSeeder
    {
        public static async Task SeedDatabase(IServiceScope serviceScope)
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.Migrate();

            if (!context.Users.Any())
            {
                // Create users
                var user1 = new ApplicationUser() { Email = "user1@gamil.com", UserName = "user1@gmail.com", Name = "Jhon", EmailConfirmed = true };
                var user2 = new ApplicationUser() { Email = "user2@gamil.com", UserName = "user2@gmail.com", Name = "Anna", EmailConfirmed = true };
                var user3 = new ApplicationUser() { Email = "user3@gamil.com", UserName = "user3@gmail.com", Name = "Carlos", EmailConfirmed = true };
                var admin = new ApplicationUser() { Email = "admin@bank.com", UserName = "admin@bank.com", Name = "Admin", EmailConfirmed = true };
                
                await userManager.CreateAsync(user1, "Test12345.");
                await userManager.CreateAsync(user2, "Test12345.");
                await userManager.CreateAsync(user3, "Test12345.");
                await userManager.CreateAsync(admin, "Test12345.");



                // Create roles
                var customerRole = new IdentityRole { Name = "customer" };
                var adminRole = new IdentityRole { Name = "admin" };
                
                await roleManager.CreateAsync(adminRole);
                await roleManager.CreateAsync(customerRole);



                // Add roles to users
                await userManager.AddToRoleAsync(user1, customerRole.Name);
                await userManager.AddToRoleAsync(user2, customerRole.Name);
                await userManager.AddToRoleAsync(user3, customerRole.Name);
                await userManager.AddToRoleAsync(admin, adminRole.Name);



                // Create bank accounts for customers
                var usersBankAccounts = new List<Account> { 
                    new Account { Alias = "Cuenta 1", Balance = 10000, Number = 1001, UserId = user1.Id },
                    new Account { Alias = "Cuenta 2", Balance = 10000, Number = 1002, UserId = user1.Id },
                    new Account { Alias = "Cuenta 3", Balance = 10000, Number = 1003, UserId = user1.Id },

                    new Account { Alias = "Cuenta 1", Balance = 10000, Number = 2001, UserId = user2.Id },
                    new Account { Alias = "Cuenta 2", Balance = 10000, Number = 2002, UserId = user2.Id },
                    new Account { Alias = "Cuenta 3", Balance = 10000, Number = 2003, UserId = user2.Id },


                    new Account { Alias = "Cuenta 1", Balance = 10000, Number = 3001, UserId = user3.Id },
                    new Account { Alias = "Cuenta 2", Balance = 10000, Number = 3002, UserId = user3.Id },
                    new Account { Alias = "Cuenta 3", Balance = 10000, Number = 3003, UserId = user3.Id }
                };


                context.Accounts.AddRange(usersBankAccounts);
                await context.SaveChangesAsync();
            }
        }
    }
}
