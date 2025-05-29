using BankApplication.Contexts;
using BankApplication.Interfaces;
using BankApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApplication.Repositories
{
    public class UserRepository : Repository<int, User>
    {
        public UserRepository(BankContext context) : base(context)
        {
        }

        public override async Task<User> Get(int key)
        {
            try
            {
                var user = await _bankContext.users.SingleOrDefaultAsync(u => u.Id == key);
                return user ?? throw new Exception("No User with the given User Id");
            }

            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user: {ex.Message}");
            }
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                var users = _bankContext.users;
                if (!users.Any())
                    throw new Exception("No users found in the database");
                return (await users.ToListAsync());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving users: {ex.Message}");
            }
        }
    }
}