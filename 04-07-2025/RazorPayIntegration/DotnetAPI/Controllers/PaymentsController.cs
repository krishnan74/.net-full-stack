using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Models;
using DotnetAPI.Models.DTOs.Payment;
using DotnetAPI.Services;
using DotnetAPI.Interfaces;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> CreatePayment([FromBody] PaymentDTO paymentDTO)
        {
            var payment = await _paymentService.CreatePaymentAsync(paymentDTO);
            return Ok(payment);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPaymentById(Guid id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return Ok(payments);
        }
    }
}
