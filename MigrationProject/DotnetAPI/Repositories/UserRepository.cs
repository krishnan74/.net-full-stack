using DotnetAPI.Models;
using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Repositories
{
    public class UserRepository : Repository<int, User>
    {
        public UserRepository(DotnetAPIContext context) : base(context) { }

        public override async Task<User> Get(int key)
        {
            var user = await _context.Users.SingleOrDefaultAsync(p => p.UserId == key);
            return user ?? throw new KeyNotFoundException($"No user with the given ID: {key}");
        }

        public override async Task<IEnumerable<User>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var users = _context.Users;
            if (!users.Any())
                return Enumerable.Empty<User>();
            return await users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
