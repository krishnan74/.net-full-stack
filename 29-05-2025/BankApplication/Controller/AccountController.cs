
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
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAccountTransactionService _accountTransactionService;

        public AccountController(IAccountService accountService, IAccountTransactionService accountTransactionService)
        {
            _accountService = accountService;
            _accountTransactionService = accountTransactionService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountAddRequestDTO account)
        {
            if (account == null)
            {
                return BadRequest("Account data is required.");
            }

            var createdAccount = await _accountService.CreateAccount(account);
            if (createdAccount == null)
            {
                return BadRequest("Account creation failed.");
            }

            if (account.MinimumBalance > 0)
            {
                var transactionRequest = new TransactionRequestDTO
                {
                    AccountNumber = createdAccount.AccountNumber,
                    Amount = account.MinimumBalance,
                    Description = "Initial deposit for account creation"
                };

                var transaction = await _accountTransactionService.Deposit(transactionRequest);
                if (transaction == null)
                {
                    return BadRequest("Failed to deposit minimum balance.");
                }
            }
            

            return Ok(createdAccount);
        }

        [HttpGet("getBalance/{accountNumber}")]
        public async Task<IActionResult> GetAccountBalance(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                return BadRequest("Account number is required.");
            }

            var balance = await _accountService.GetAccountBalance(accountNumber);
            if (balance == null)
            {
                return NotFound("Account not found.");
            }

            return Ok(balance);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAccountsByUser(int userId)
        {
            var accounts = await _accountService.GetAccountsByUser(userId);
            if (accounts == null || !accounts.Any())
            {
                return NotFound("No accounts found for the specified user.");
            }

            return Ok(accounts);
        }

        [HttpPost("deactivate/{accountNumber}")]
        public async Task<IActionResult> DeactivateAccount(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                return BadRequest("Account number is required.");
            }

            var result = await _accountService.DeactivateAccount(accountNumber);
            if (!result)
            {
                return BadRequest("Account deactivation failed.");
            }

            return Ok("Account deactivated successfully.");
        }

        [HttpPost("activate/{accountNumber}")]
        public async Task<IActionResult> ActivateAccount(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                return BadRequest("Account number is required.");
            }

            var result = await _accountService.ActivateAccount(accountNumber);
            if (!result)
            {
                return BadRequest("Account activation failed.");
            }

            return Ok("Account activated successfully.");
        }

        [HttpPost("change-type/{accountNumber}")]
        public async Task<IActionResult> ChangeAccountType(string accountNumber, [FromBody] string newAccountType)
        {
            if (string.IsNullOrEmpty(accountNumber) || string.IsNullOrEmpty(newAccountType))
            {
                return BadRequest("Account number and new account type are required.");
            }

            var result = await _accountService.ChangeAccountType(accountNumber, newAccountType);
            if (!result)
            {
                return BadRequest("Failed to change account type.");
            }

            return Ok("Account type changed successfully.");
        }

        [HttpPost("change-minimum-balance/{accountNumber}")]
        public async Task<IActionResult> ChangeMinimumBalance(string accountNumber, [FromBody] decimal newMinimumBalance)
        {
            if (string.IsNullOrEmpty(accountNumber) || newMinimumBalance < 0)
            {
                return BadRequest("Account number and new minimum balance are required.");
            }

            var result = await _accountService.ChangeMinimumBalance(accountNumber, newMinimumBalance);
            if (!result)
            {
                return BadRequest("Failed to change minimum balance.");
            }

            return Ok("Minimum balance changed successfully.");
        }



    }
}