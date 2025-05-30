
using System.Threading.Tasks;
using BankApplication.Interfaces;
using BankApplication.Models;
using BankApplication.Models.DTO;
using BankApplication.Services;
using Microsoft.AspNetCore.Mvc;


namespace BankApplication.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountTransactionController : ControllerBase
    {
        private readonly IAccountTransactionService _accountTransactionService;

        public AccountTransactionController(IAccountTransactionService accountTransactionService)
        {
            _accountTransactionService = accountTransactionService;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] TransactionRequestDTO transactionRequest)
        {
            if (transactionRequest == null || string.IsNullOrEmpty(transactionRequest.AccountNumber) || transactionRequest.Amount <= 0)
            {
                return BadRequest("Invalid transaction data.");
            }

            try
            {
                var transaction = await _accountTransactionService.Deposit(transactionRequest);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error during deposit: {ex.Message}");
            }
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] TransactionRequestDTO transactionRequest)
        {
            if (transactionRequest == null || string.IsNullOrEmpty(transactionRequest.AccountNumber) || transactionRequest.Amount <= 0)
            {
                return BadRequest("Invalid transaction data.");
            }

            try
            {
                var transaction = await _accountTransactionService.Withdraw(transactionRequest);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error during withdrawal: {ex.Message}");
            }
        }

        [HttpGet("transactions/{accountNumber}")]
        public async Task<IActionResult> GetTransactionsByAccount(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                return BadRequest("Account number is required.");
            }

            try
            {
                var transactions = await _accountTransactionService.GetTransactionsByAccountNumber(accountNumber);
                if (transactions == null || !transactions.Any())
                {
                    return NotFound("No transactions found for the specified account.");
                }

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving transactions: {ex.Message}");
            }
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferRequestDTO transferRequest)
        {
            if (transferRequest == null || string.IsNullOrEmpty(transferRequest.FromAccountNumber) || string.IsNullOrEmpty(transferRequest.ToAccountNumber) || transferRequest.Amount <= 0)
            {
                return BadRequest("Invalid transfer data.");
            }

            try
            {
                var transaction = await _accountTransactionService.Transfer(transferRequest);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error during transfer: {ex.Message}");
            }
        }
    }
}