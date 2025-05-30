using BankApplication.Models;
using BankApplication.Models.DTO;

namespace BankApplication.Misc
{
    public class AccountMapper
    {
        public Account? MapAccountAddRequest(AccountAddRequestDTO addRequestDto)
        {
            Account account = new();
            account.AccountNumber = GenerateAccountNumber();
            account.IFSCCode = GenerateIFSCCode(addRequestDto.BankId);
            account.Balance = 0;
            account.AccountType = addRequestDto.AccountType;
            account.Status = "Active";
            account.MinimumBalance = addRequestDto.MinimumBalance;
            account.UserId = addRequestDto.UserId;
            account.BankId = addRequestDto.BankId;

            return account;
        }

        public string GenerateAccountNumber()
        {
            Random random = new Random();
            string accountNumber = "AC" + random.Next(10000000, 99999999).ToString();
            return accountNumber;
        }
        
        public string GenerateIFSCCode(int bankId)
        {
            Random random = new Random();
            string ifscCode = "IFSC" + bankId.ToString() + random.Next(1000, 9999).ToString();
            return ifscCode;
        }
    }
}