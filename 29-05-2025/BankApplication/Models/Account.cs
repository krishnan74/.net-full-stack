namespace BankApplication.Models
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public decimal Balance { get; set; }
        public string AccountType { get; set; }
        public string Status { get; set; }
        public decimal MinimumBalance { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int BankId { get; set; }
        public Bank? Bank { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
}