using Razorpay.Api;
using System.Collections.Generic;

namespace DotnetAPI.Interfaces
{
    public interface IRazorpayService
    {
        public Order CreateOrder(int amountInPaise);
    }
}