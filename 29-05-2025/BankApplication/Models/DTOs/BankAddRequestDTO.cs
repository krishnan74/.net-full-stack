namespace BankApplication.Models.DTO
{
    public class BankAddRequestDTO
    {
        public string Name { get; set; }
        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<BranchAddRequestDTO>? BankBranches { get; set; } = new List<BranchAddRequestDTO>();
    }
}