
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
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBank([FromBody] BankAddRequestDTO bankAddRequest)
        {
            if (bankAddRequest == null)
            {
                return BadRequest("Bank data is required.");
            }

            var createdBank = await _bankService.CreateBank(bankAddRequest);
            if (createdBank == null)
            {
                return BadRequest("Bank creation failed.");
            }

            return Ok(createdBank);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBanks()
        {
            var banks = await _bankService.GetAllBanks();
            if (banks == null || !banks.Any())
            {
                return NotFound("No banks found.");
            }

            return Ok(banks);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateBankDetails([FromBody] BankUpdateRequestDTO bankUpdateRequest)
        {
            if (bankUpdateRequest == null)
            {
                return BadRequest("Bank update data is required.");
            }

            var updatedBank = await _bankService.UpdateBankDetails(bankUpdateRequest);
            if (updatedBank == null)
            {
                return BadRequest("Bank update failed.");
            }

            return Ok(updatedBank);
        }

        [HttpDelete("delete/{bankId}")]
        public async Task<IActionResult> DeleteBank(int bankId)
        {
            if (bankId <= 0)
            {
                return BadRequest("Valid bank ID is required.");
            }

            var deletedBank = await _bankService.DeleteBank(bankId);
            if (deletedBank == null)
            {
                return NotFound("Bank not found or deletion failed.");
            }

            return Ok(deletedBank);
        }

    }
}