using BankApplication.Models.DTO;
using BankApplication.Models;

namespace BankApplication.Interfaces
{
    public interface IBranchService
    {
        Task<Branch> CreateBranch(BranchAddRequestDTO branch);
        Task<IEnumerable<Branch>> GetBranchesByBank(int bankId);
        Task<Branch> DeleteBranch(int branchCode);

    }
}