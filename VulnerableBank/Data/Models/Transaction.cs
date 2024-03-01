using System.ComponentModel.DataAnnotations.Schema;

namespace VulnerableBank.Data.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [ForeignKey("SourceAccount")]
        public int SourceAccountNumber { get; set; }
        public Account SourceAccount { get; set; }

        [ForeignKey("DestinationAccount")]
        public int DestinationAccountNumber { get; set; }
        public Account DestinationAccount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}
