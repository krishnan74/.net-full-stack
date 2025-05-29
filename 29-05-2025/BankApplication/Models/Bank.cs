namespace BankApplication.Models
{
    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<Account>? BankAccounts { get; set; } 
        public ICollection<Branch>? BankBranches { get; set; }
    }
}