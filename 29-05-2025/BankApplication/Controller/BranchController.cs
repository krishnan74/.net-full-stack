
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
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBranch([FromBody] BranchAddRequestDTO branch)
        {
            if (branch == null)
            {
                return BadRequest("Branch data is required.");
            }

            var createdBranch = await _branchService.CreateBranch(branch);
            if (createdBranch == null)
            {
                return BadRequest("Branch creation failed.");
            }

            return Ok(createdBranch);
        }

        // get all branches of a bank
        [HttpGet("bank/{bankId}")]
        public async Task<IActionResult> GetBranchesByBank(int bankId)
        {
            if (bankId <= 0)
            {
                return BadRequest("Invalid bank ID.");
            }

            var branches = await _branchService.GetBranchesByBank(bankId);
            if (branches == null || !branches.Any())
            {
                return NotFound("No branches found for the specified bank.");
            }

            return Ok(branches);
        }

        //delete
        [HttpDelete("delete/{branchId}")]
        public async Task<IActionResult> DeleteBranch(int branchId)
        {
            if (branchId <= 0)
            {
                return BadRequest("Invalid branch ID.");
            }

            var deletedBranch = await _branchService.DeleteBranch(branchId);
            
            if (deletedBranch == null)
            {
                return NotFound("Branch not found.");
            }

            return Ok("Branch deleted successfully.");
        }

    }

}