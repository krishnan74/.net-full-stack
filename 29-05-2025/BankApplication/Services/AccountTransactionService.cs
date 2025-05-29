using Microsoft.EntityFrameworkCore;
using BankApplication.Models;
using BankApplication.Models.DTO;
using BankApplication.Interfaces;
using BankApplication.Contexts;
using BankApplication.Misc;

namespace BankApplication.Services
{

    public class AccountTransactionService : IAccountTransactionService
    {

        private readonly BankContext _bankContext;
        public AccountTransactionService(BankContext bankContext)
        {
            _bankContext = bankContext;
        }

        public async Task<Transaction> Deposit(string accountNumber, decimal amount, string description)
        {
            using var transaction = await _bankContext.Database.BeginTransactionAsync();

            try
            {
                var accounts = _bankContext.accounts;
                var account = accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

                if (account == null)
                {
                    throw new Exception("Account not found.");
                }

                if (amount <= 0)
                {
                    throw new Exception("Deposit amount must be greater than zero.");
                }

                account.Balance += amount;
                await _bankContext.SaveChangesAsync();

                var createdTransaction = new Transaction
                {
                    AccountNumber = accountNumber,
                    Amount = amount,
                    Description = description,
                    TransactionType = "Deposit",
                    TransactionDate = DateTime.Now
                };

                await _bankContext.AddAsync(createdTransaction);

                await transaction.CommitAsync();
                return createdTransaction; // Assuming you have a way to save this transaction
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                throw new Exception($"Error during deposit: {ex.Message}");
            }
        }

        public async Task<Transaction> Withdraw(string accountNumber, decimal amount, string description)
        {
            using var transaction = await _bankContext.Database.BeginTransactionAsync();

            try
            {
                var accounts = _bankContext.accounts;
                var account = accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

                if (account == null)
                {
                    throw new Exception("Account not found.");
                }

                if (amount <= 0)
                {
                    throw new Exception("Withdrawal amount must be greater than zero.");
                }

                if (account.Balance < amount)
                {
                    throw new Exception("Insufficient funds for withdrawal.");
                }

                account.Balance -= amount;
                await _bankContext.SaveChangesAsync();

                var createdTransaction = new Transaction
                {
                    AccountNumber = accountNumber,
                    Amount = -amount,
                    Description = description,
                    TransactionType = "Withdrawal",
                    TransactionDate = DateTime.Now
                };

                await _bankContext.AddAsync(createdTransaction);

                await transaction.CommitAsync();
                return createdTransaction; // Assuming you have a way to save this transaction
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                throw new Exception($"Error during withdrawal: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountNumber(string accountNumber)
        {
            try
            {
                var transactions = await _bankContext.transactions
                    .Where(t => t.AccountNumber == accountNumber)
                    .ToListAsync();

                return transactions;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving transactions: {ex.Message}");
            }
        }

        public async Task<Transaction> Transfer(string fromAccountNumber, string toAccountNumber, decimal amount, string description)
        {
            using var transaction = await _bankContext.Database.BeginTransactionAsync();

            try
            {
                var fromAccount = await _bankContext.accounts.FirstOrDefaultAsync(a => a.AccountNumber == fromAccountNumber);
                var toAccount = await _bankContext.accounts.FirstOrDefaultAsync(a => a.AccountNumber == toAccountNumber);

                if (fromAccount == null || toAccount == null)
                {
                    throw new Exception("One or both accounts not found.");
                }

                if (amount <= 0)
                {
                    throw new Exception("Transfer amount must be greater than zero.");
                }

                if (fromAccount.Balance < amount)
                {
                    throw new Exception("Insufficient funds for transfer.");
                }

                fromAccount.Balance -= amount;
                toAccount.Balance += amount;

                await _bankContext.SaveChangesAsync();

                var createdTransaction = new Transaction
                {
                    AccountNumber = fromAccountNumber,
                    TransferAccountNumber = toAccountNumber,
                    Amount = -amount,
                    Description = description,
                    TransactionType = "Transfer",
                    TransactionDate = DateTime.Now
                };

                await _bankContext.AddAsync(createdTransaction);
                
                await _bankContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return createdTransaction; // Assuming you have a way to save this transaction
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                throw new Exception($"Error during transfer: {ex.Message}");
            }
        }
 
    }
}