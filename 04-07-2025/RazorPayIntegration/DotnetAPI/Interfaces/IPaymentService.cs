
using DotnetAPI.Models;
using DotnetAPI.Models.DTOs.Payment;
namespace DotnetAPI.Interfaces
{
    public interface IPaymentService
    {
        public Task<Payment> CreatePaymentAsync(PaymentDTO paymentDTO);
        public Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        public Task<Payment> GetPaymentByIdAsync(Guid id);
    }
}