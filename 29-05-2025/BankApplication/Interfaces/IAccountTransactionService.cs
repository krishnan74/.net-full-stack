using BankApplication.Models.DTO;
using BankApplication.Models;

namespace BankApplication.Interfaces
{
    public interface IAccountTransactionService
    {
        Task<Transaction> Deposit(TransactionRequestDTO transactionRequestDTO);
        Task<Transaction> Withdraw(TransactionRequestDTO transactionRequestDTO);
        Task<Transaction> Transfer(TransferRequestDTO transferRequestDTO);
        Task<IEnumerable<Transaction>> GetTransactionsByAccountNumber(string accountNumber);
        
    }
}