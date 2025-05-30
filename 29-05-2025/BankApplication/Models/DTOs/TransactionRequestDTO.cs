namespace BankApplication.Models.DTO
{
    public class TransactionRequestDTO
    {
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}