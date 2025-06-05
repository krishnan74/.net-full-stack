using Notify.Contexts;
using Notify.Interfaces;
using Notify.Models;
using Microsoft.EntityFrameworkCore;

namespace Notify.Repositories
{
    public class UserRepository : Repository<string, User>
    {
        public UserRepository(NotifyContext notifyContext):base(notifyContext)
        {
            
        }
        public override async Task<User> Get(string key)
        {
            return await _notifyContext.Users.SingleOrDefaultAsync(u => u.Username == key);
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await _notifyContext.Users.ToListAsync();
        }
            
    }
}