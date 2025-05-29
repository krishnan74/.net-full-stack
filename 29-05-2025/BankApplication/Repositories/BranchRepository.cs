using BankApplication.Contexts;
using BankApplication.Interfaces;
using BankApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApplication.Repositories
{
    public class BranchRepository : Repository<int, Branch>
    {
        public BranchRepository(BankContext context) : base(context)
        {
        }

        public override async Task<Branch> Get(int key)
        {
            try
            {
                var branch = await _bankContext.branches.SingleOrDefaultAsync(br => br.BranchCode == key);
                return branch ?? throw new Exception("No Branch with the given Branch Code");
            }

            catch (Exception ex)
            {
                throw new Exception($"Error retrieving branch: {ex.Message}");
            }
        }

        public override async Task<IEnumerable<Branch>> GetAll()
        {
            try
            {
                var branches = _bankContext.branches;
                if (!branches.Any())
                    throw new Exception("No branches found in the database");
                return (await branches.ToListAsync());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving branchs: {ex.Message}");
            }
        }
    }
}