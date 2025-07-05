namespace DotnetAPI.Models.DTOs.Order
{
    public class OrderDTO
    {
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Amount { get; set; } = 0;
        public string ContactNumber { get; set; } = string.Empty;
    }
}