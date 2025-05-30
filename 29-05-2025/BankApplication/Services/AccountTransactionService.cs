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

        public async Task<Transaction> Deposit(TransactionRequestDTO transactionRequestDTO)
        {
            using var transaction = await _bankContext.Database.BeginTransactionAsync();
            
            var accountNumber = transactionRequestDTO.AccountNumber;
            var amount = transactionRequestDTO.Amount;
            var description = transactionRequestDTO.Description;

            try
            {
                var accounts = _bankContext.accounts;
                var account = await accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

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
                await _bankContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return createdTransaction;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                throw new Exception($"Error during deposit: {ex.Message}");
            }
        }

        public async Task<Transaction> Withdraw(TransactionRequestDTO transactionRequestDTO)
        {
            using var transaction = await _bankContext.Database.BeginTransactionAsync();

            var accountNumber = transactionRequestDTO.AccountNumber;
            var amount = transactionRequestDTO.Amount;
            var description = transactionRequestDTO.Description;

            try
            {
                var accounts = _bankContext.accounts;
                var account = await accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

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

                if( account.MinimumBalance > (account.Balance - amount))
                {
                    throw new Exception("Withdrawal amount must be lower minimum balance.");
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
                await _bankContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return createdTransaction;
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

        public async Task<Transaction> Transfer(TransferRequestDTO transferRequestDTO)
        {
            using var transaction = await _bankContext.Database.BeginTransactionAsync();

            var fromAccountNumber = transferRequestDTO.FromAccountNumber;
            var toAccountNumber = transferRequestDTO.ToAccountNumber;
            var amount = transferRequestDTO.Amount;
            var description = transferRequestDTO.Description;

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
                    Amount = amount,
                    Description = description,
                    TransactionType = "Transfer",
                    TransactionDate = DateTime.Now
                };

                await _bankContext.AddAsync(createdTransaction);

                await _bankContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return createdTransaction;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                throw new Exception($"Error during transfer: {ex.Message}");
            }
        }
 
    }
}