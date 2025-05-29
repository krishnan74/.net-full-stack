using BankApplication.Contexts;
using BankApplication.Interfaces;
using BankApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApplication.Repositories
{
    public class AccountRepository : Repository<string, Account>
    {
        public AccountRepository(BankContext context) : base(context)
        {
        }

        public override async Task<Account> Get(string key)
        {
            try
            {
                var account = await _bankContext.accounts.SingleOrDefaultAsync(a => a.AccountNumber == key);
                return account ?? throw new Exception("No Account with the given Account Number");
            }

            catch (Exception ex)
            {
                throw new Exception($"Error retrieving account: {ex.Message}");
            }
        }

        public override async Task<IEnumerable<Account>> GetAll()
        {
            try
            {
                var accounts = _bankContext.accounts;
                if (!accounts.Any())
                    throw new Exception("No accounts found in the database");
                return (await accounts.ToListAsync());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving accounts: {ex.Message}");
            }
        }
    }
}