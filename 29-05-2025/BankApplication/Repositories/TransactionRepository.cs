using BankApplication.Contexts;
using BankApplication.Interfaces;
using BankApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApplication.Repositories
{
    public class TransactionRepository : Repository<int, Transaction>
    {
        public TransactionRepository(BankContext context) : base(context)
        {
        }

        public override async Task<Transaction> Get(int key)
        {
            try
            {
                var transaction = await _bankContext.transactions.SingleOrDefaultAsync(t => t.Id == key);
                return transaction ?? throw new Exception("No Transaction with the given Transaction Id");
            }

            catch (Exception ex)
            {
                throw new Exception($"Error retrieving transaction: {ex.Message}");
            }
        }

        public override async Task<IEnumerable<Transaction>> GetAll()
        {
            try
            {
                var transactions = _bankContext.transactions;
                if (!transactions.Any())
                    throw new Exception("No transactions found in the database");
                return (await transactions.ToListAsync());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving transactions: {ex.Message}");
            }
        }
    }
}