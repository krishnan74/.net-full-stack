using BankApplication.Models;
using BankApplication.Models.DTO;

namespace BankApplication.Misc
{
    public class BranchMapper
    {
        public Branch? MapBranchAddRequest(BranchAddRequestDTO addRequestDto)
        {
            Branch branch = new();
            branch.BranchName = addRequestDto.BranchName;
            branch.BranchCode = addRequestDto.BranchCode;
            branch.BankId = addRequestDto.BankId;
            return branch;
        }
    }
}