namespace BankApplication.Models.DTO
{
    public class BankAddRequestDTO
    {
        public string Name { get; set; }
        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<BranchAdd>? BankBranches { get; set; } = new List<BranchAdd>();
    }

    public class BranchAdd
    {
        public string BranchName { get; set; } = string.Empty;
        public int BranchCode { get; set; }
    }
}