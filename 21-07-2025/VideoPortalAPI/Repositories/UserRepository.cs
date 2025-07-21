using VideoPortalAPI.Contexts;
using VideoPortalAPI.Interfaces;
using VideoPortalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace VideoPortalAPI.Repositories
{
    public class UserRepository : Repository<string, User>
    {
        public UserRepository(VideoPortalContext context):base(context)
        {
            
        }
        public override async Task<User> GetById(string key)
        {
            return await _videoPortalContext.Users.SingleOrDefaultAsync(u => u.Username == key);
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await _videoPortalContext.Users.ToListAsync();
        }
            
    }
}