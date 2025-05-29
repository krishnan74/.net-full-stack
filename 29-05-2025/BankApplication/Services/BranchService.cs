using Microsoft.EntityFrameworkCore;
using BankApplication.Models;
using BankApplication.Models.DTO;
using BankApplication.Interfaces;
using BankApplication.Misc;

namespace BankApplication.Services
{
    public class BranchService : IBranchService
    {

        private readonly IRepository<int, Branch> _branchRepository;

        public BranchService(IRepository<int, Branch> branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Branch> CreateBranch(Branch branch)
        {
            try
            {
                var branches = await _branchRepository.GetAll();
                var existingBranch = branches.FirstOrDefault(b => b.BranchCode == branch.BranchCode && b.BankId == branch.BankId);
                if (existingBranch != null)
                {
                    throw new Exception("Branch already exists.");
                }

                var createdBranch = await _branchRepository.Add(branch);
                return createdBranch ?? throw new Exception("Branch creation failed");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding branch: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Branch>> GetBranchesByBank(int bankId)
        {
            try
            {
                var branches = await _branchRepository.GetAll();
                var bankBranches = branches.Where(b => b.BankId == bankId).ToList();
                if (!bankBranches.Any())
                {
                    throw new Exception("No branches found for the given bank.");
                }
                return bankBranches;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving branches: {ex.Message}");
            }
        }

        public async Task<Branch> DeleteBranch(int branchCode)
        {
            try
            {
                var branch = await _branchRepository.Get(branchCode);
                if (branch == null)
                {
                    throw new Exception("Branch not found.");
                }
                return await _branchRepository.Delete(branchCode);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting branch: {ex.Message}");
            }
        }

    }
}