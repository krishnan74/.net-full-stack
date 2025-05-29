namespace BankApplication.Models.DTO
{
    public class BankUpdateRequestDTO
    {
        public int BankId { get; set; }
        public string BankName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}