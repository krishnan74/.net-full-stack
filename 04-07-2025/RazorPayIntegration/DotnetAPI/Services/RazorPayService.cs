using Razorpay.Api;
using System.Collections.Generic;
using DotnetAPI.Interfaces;
namespace DotnetAPI.Services
{
    public class RazorpayService : IRazorpayService
    {
        private const string key = "rzp_test_4m79LmejWoUq7j";
        private const string secret = "5eJxb1NBMaqrEpqnC92ihD9O";

        public Order CreateOrder(int amountInPaise)
        {
            RazorpayClient client = new RazorpayClient(key, secret);

            Dictionary<string, object> options = new Dictionary<string, object>
        {
            { "amount", amountInPaise }, 
            { "currency", "INR" },
            { "receipt", "receipt#1" },
            { "payment_capture", 1 }
        };

            Order order = client.Order.Create(options);
            return order;
        }
    }
}