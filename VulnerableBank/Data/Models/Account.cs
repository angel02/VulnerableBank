using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VulnerableBank.Data.Models
{
    public class Account
    {
        [Key]
        public int Number { get; set; }

        public string Alias { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }

        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
