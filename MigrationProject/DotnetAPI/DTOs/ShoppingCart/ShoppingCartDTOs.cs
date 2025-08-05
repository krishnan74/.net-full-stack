namespace DotnetAPI.DTOs.ShoppingCart
{
    public class AddToCartDTO
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class UpdateCartDTO
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class CheckoutDTO
    {
        public int UserId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
    }
    public class PaypalPaymentDTO
    {
        public string PaymentId { get; set; }
        public string PayerId { get; set; }
    }
}
