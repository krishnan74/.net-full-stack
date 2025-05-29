namespace BankApplication.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string Description { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public Account? Account { get; set; }
        public string TransferAccountNumber { get; set; } = string.Empty;
        public Account? TransferAccount { get; set; }
    }
}