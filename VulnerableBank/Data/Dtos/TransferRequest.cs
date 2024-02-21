namespace VulnerableBank.Data.Dtos
{
    public class TransferRequest
    {
        public int AccountSource { get; set; }
        public int AccountDestination { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = String.Empty;
    }
}
