using Microsoft.EntityFrameworkCore;
using BankApplication.Models;
using BankApplication.Models.DTO;
using BankApplication.Interfaces;
using BankApplication.Misc;

namespace BankApplication.Services
{
    public class BankService : IBankService
    {

        BankMapper bankMapper;
        private readonly IRepository<int, Bank> _bankRepository;
        private readonly IRepository<int, Branch> _branchRepository;

        public BankService(IRepository<int, Bank> bankRepository, IRepository<int, Branch> branchRepository)
        {
            _bankRepository = bankRepository;
            _branchRepository = branchRepository;
            bankMapper = new BankMapper();
        }

        public async Task<Bank> CreateBank(BankAddRequestDTO bank)
        {
            try
            {
                var newBank = bankMapper.MapBankAddRequest(bank);
                var createdBank = await _bankRepository.Add(newBank);

                foreach(var branch in bank.BankBranches)
                {
                    var newBranch = new Branch
                    {
                        BranchName = branch.BranchName,
                        BankId = createdBank.Id
                    };

                    var createdBranch = await _branchRepository.Add(newBranch);
                    if (createdBranch == null)
                    {
                        throw new Exception($"Failed to create branch: {branch.BranchName}");
                    }
                }

                return createdBank ?? throw new Exception("Bank creation failed");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating bank: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Bank>> GetAllBanks()
        {
            try
            {
                var banks = await _bankRepository.GetAll();
                if (!banks.Any())
                {
                    throw new Exception("No banks found in the database");
                }
                return banks;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving banks: {ex.Message}");
            }
        }
        public async Task<Bank> UpdateBankDetails(BankUpdateRequestDTO bankUpdateRequest)
        {
            try
            {
                var bank = await _bankRepository.Get(bankUpdateRequest.BankId);
                if (bank == null)
                {
                    throw new Exception("Bank not found.");
                }

                bank.Name = bankUpdateRequest.BankName;
                bank.ContactNumber = bankUpdateRequest.ContactNumber;
                bank.Email = bankUpdateRequest.Email;

                var updatedBank = await _bankRepository.Update(bank.Id, bank);
                return updatedBank;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating bank details: {ex.Message}");
            }
        }
        public async Task<Bank> DeleteBank(int bankId)
        {
            try
            {
                var bank = await _bankRepository.Get(bankId);
                if (bank == null)
                {
                    throw new Exception("Bank not found.");
                }

                var deleted = await _bankRepository.Delete(bankId);
                return deleted;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting bank: {ex.Message}");
            }
        }

    }
}