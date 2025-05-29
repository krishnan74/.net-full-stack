using BankApplication.Models.DTO;
using BankApplication.Models;

namespace BankApplication.Interfaces
{
    public interface IAccountTransactionService
    {
        Task<Transaction> Deposit(string accountNumber, decimal amount, string description);
        Task<Transaction> Withdraw(string accountNumber, decimal amount, string description);
        Task<Transaction> Transfer(string fromAccountNumber, string toAccountNumber, decimal amount, string description);
        Task<IEnumerable<Transaction>> GetTransactionsByAccountNumber(string accountNumber);
        
    }
}