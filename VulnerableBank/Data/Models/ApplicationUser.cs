using Microsoft.AspNetCore.Identity;

namespace VulnerableBank.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
