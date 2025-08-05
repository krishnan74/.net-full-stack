using System.Collections.Generic;

namespace DotnetAPI.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
