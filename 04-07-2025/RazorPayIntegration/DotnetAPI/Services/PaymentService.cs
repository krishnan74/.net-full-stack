using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.Contexts;
using DotnetAPI.Models.DTOs.Payment;

namespace DotnetAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Guid, Payment> _paymentRepository;

        private readonly DatabaseContext _context;
        public PaymentService(IRepository<Guid, Payment> paymentRepository, DatabaseContext context)
        {
            _paymentRepository = paymentRepository;
            _context = context;
        }

        public async Task<Payment> CreatePaymentAsync(PaymentDTO paymentDTO)
        {
            try
            {

                if (paymentDTO.Amount <= 0)
                {
                    throw new ArgumentException("Payment amount must be greater than zero.");
                }   

                if (string.IsNullOrWhiteSpace(paymentDTO.Currency) || string.IsNullOrWhiteSpace(paymentDTO.Status))
                {
                    throw new ArgumentException("Payment currency and status cannot be null or empty.");
                }

                var newPayment = new Payment
                {
                    Id = Guid.NewGuid(),
                    Amount = paymentDTO.Amount,
                    Currency = paymentDTO.Currency,
                    Status = paymentDTO.Status,
                    OrderId = paymentDTO.OrderId
                };

                if (newPayment == null)
                {
                    throw new Exception("Failed to map PaymentDTO to Payment.");
                }

                var addedPayment = await _paymentRepository.Add(newPayment);

                if (addedPayment == null)
                {
                    throw new Exception("Failed to add new payment.");
                }

                return addedPayment;

            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Invalid payment data: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the payment.", ex);
            }
        }

        public async Task<Payment> GetPaymentByIdAsync(Guid id)
        {
            try
            {
                return await _paymentRepository.Get(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Payment with ID {id} not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the payment with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
             try
            {
                return await _paymentRepository.GetAll();

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all payments.", ex);
            }
        }
    
    }
}