using BankApplication.Contexts;
using BankApplication.Interfaces;
using BankApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApplication.Repositories
{
    public class BankRepository : Repository<int, Bank>
    {
        public BankRepository(BankContext context) : base(context)
        {
        }

        public override async Task<Bank> Get(int key)
        {
            try
            {
                var bank = await _bankContext.banks.SingleOrDefaultAsync(b => b.Id == key);
                return bank ?? throw new Exception("No Bank with the given Bank Id");
            }

            catch (Exception ex)
            {
                throw new Exception($"Error retrieving bank: {ex.Message}");
            }
        }

        public override async Task<IEnumerable<Bank>> GetAll()
        {
            try
            {
                var banks = _bankContext.banks;
                if (!banks.Any())
                    throw new Exception("No banks found in the database");
                return (await banks.ToListAsync());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving banks: {ex.Message}");
            }
        }
    }
}