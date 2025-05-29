namespace BankApplication.Models
{
    public class Branch
    {
        public int BranchCode { get; set; }
        public string BranchName { get; set; } = string.Empty;   
        public int BankId { get; set; }
        public Bank? Bank { get; set; }
    }
}