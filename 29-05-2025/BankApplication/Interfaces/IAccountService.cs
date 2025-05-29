using BankApplication.Models.DTO;
using BankApplication.Models;

namespace BankApplication.Interfaces
{
    public interface IAccountService
    {
        Task<Account> CreateAccount(AccountAddRequestDTO account);
        Task<IEnumerable<Account>> GetAccountsByUser(int userId);
        Task<bool> DeactivateAccount(string accountNumber);
        Task<bool> ActivateAccount(string accountNumber);
        Task<bool> ChangeAccountType(string accountNumber, string newAccountType);
        Task<bool> ChangeMinimumBalance(string accountNumber, decimal newMinimumBalance);

   
    }
}