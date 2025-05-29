using BankApplication.Models;
using BankApplication.Models.DTO;

namespace BankApplication.Misc
{
    public class AccountMapper
    {
        public Account? MapAccountAddRequest(AccountAddRequestDTO addRequestDto)
        {
            Account account = new();
            account.AccountType = addRequestDto.AccountType;
            account.MinimumBalance = addRequestDto.MinimumBalance;
            account.UserId = addRequestDto.UserId;
            account.BankId = addRequestDto.BankId;
            
            return account;
        }
    }
}