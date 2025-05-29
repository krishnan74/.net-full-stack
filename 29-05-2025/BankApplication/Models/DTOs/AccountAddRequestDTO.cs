namespace BankApplication.Models.DTO
{
    public class AccountAddRequestDTO
    {
        public string AccountType { get; set; }
        public decimal MinimumBalance { get; set; }
        public int UserId { get; set; }
        public int BankId { get; set; }
    }
}