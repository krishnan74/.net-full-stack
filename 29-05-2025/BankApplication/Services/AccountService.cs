using Microsoft.EntityFrameworkCore;
using BankApplication.Models;
using BankApplication.Models.DTO;
using BankApplication.Interfaces;
using BankApplication.Misc;

namespace BankApplication.Services
{
    public class AccountService : IAccountService
    {

        AccountMapper accountMapper;
        private readonly IRepository<string, Account> _accountRepository;

        public AccountService(IRepository<string, Account> accountRepository)
        {
            _accountRepository = accountRepository;
            accountMapper = new AccountMapper();
        }

        public async Task<Account> CreateAccount(AccountAddRequestDTO account)
        {
            try
            {
                var newAccount = accountMapper.MapAccountAddRequest(account);

                var createdAccount = await _accountRepository.Add(newAccount);

                return createdAccount ?? throw new Exception("Account creation failed");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating account: {ex.Message}");
            }
        }

        public async Task<decimal> GetAccountBalance(string accountNumber)
        {
            try
            {
                var account = await _accountRepository.Get(accountNumber);
                if (account == null)
                {
                    throw new Exception("Account not found.");
                }
                return account.Balance;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving account balance: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Account>> GetAccountsByUser(int userId)
        {
            try
            {
                var accounts = await _accountRepository.GetAll();
                var userAccounts = accounts.Where(a => a.UserId == userId).ToList();
                if (!userAccounts.Any())
                {
                    throw new Exception("No accounts found for the given user.");
                }
                return userAccounts;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving accounts: {ex.Message}");
            }
        }


        public async Task<bool> DeactivateAccount(string accountNumber)
        {
            try
            {
                var account = await _accountRepository.Get(accountNumber);
                if (account == null)
                {
                    throw new Exception("Account not found.");
                }

                account.Status = "Deactivated";
                await _accountRepository.Update(accountNumber, account);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deactivating account: {ex.Message}");
            }
        }

        public async Task<bool> ActivateAccount(string accountNumber)
        {
            try
            {
                var account = await _accountRepository.Get(accountNumber);
                if (account == null)
                {
                    throw new Exception("Account not found.");
                }

                account.Status = "Active";
                await _accountRepository.Update(accountNumber, account);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error activating account: {ex.Message}");
            }
        }
        public async Task<bool> ChangeAccountType(string accountNumber, string newAccountType)
        {
            try
            {
                var account = await _accountRepository.Get(accountNumber);
                if (account == null)
                {
                    throw new Exception("Account not found.");
                }

                account.AccountType = newAccountType;
                await _accountRepository.Update(accountNumber, account);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error changing account type: {ex.Message}");
            }
        }
        public async Task<bool> ChangeMinimumBalance(string accountNumber, decimal newMinimumBalance)
        {
            try
            {
                var account = await _accountRepository.Get(accountNumber);
                if (account == null)
                {
                    throw new Exception("Account not found.");
                }

                account.MinimumBalance = newMinimumBalance;
                await _accountRepository.Update(accountNumber, account);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error changing minimum balance: {ex.Message}");
            }
        }
        

    }
}