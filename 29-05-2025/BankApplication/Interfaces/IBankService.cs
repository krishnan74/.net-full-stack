using BankApplication.Models;
using BankApplication.Models.DTO;

namespace BankApplication.Interfaces
{
    public interface IBankService
    {
        Task<Bank> CreateBank(BankAddRequestDTO bank);
        Task<IEnumerable<Bank>> GetAllBanks();
        Task<bool> UpdateBankDetails(BankUpdateRequestDTO bankUpdateRequest);
        Task<bool> DeleteBank(int bankId);
    }
}