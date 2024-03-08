using Microsoft.EntityFrameworkCore;
using VulnerableBank.Data;
using VulnerableBank.Data.Models;
using Microsoft.AspNetCore.Identity;
using VulnerableBank.Data.Seeder;

var builder = WebApplication.CreateBuilder(args);

var dbServer = Environment.GetEnvironmentVariable("DB_ADDRESS") ?? "host.docker.internal";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "VulnerableBank";
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

var connection = $"Server={dbServer};Database={dbName}; User Id={dbUser}; Password={dbPassword};TrustServerCertificate=True;MultipleActiveResultSets=true";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connection));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    

    // Remove for demo
    //app.UseHsts();
}
var scope = app.Services.CreateScope();
MasterSeeder.SeedDatabase(scope).Wait();

// Remove for demo
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
