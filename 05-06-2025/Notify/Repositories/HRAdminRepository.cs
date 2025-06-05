using Notify.Contexts;
using Notify.Interfaces;
using Notify.Models;
using Microsoft.EntityFrameworkCore;

namespace Notify.Repositories
{
    public class HRAdminRepository : Repository<int, HRAdmin>
    {
        public HRAdminRepository(NotifyContext notifyContext):base(notifyContext)
        {
            
        }
        public override async Task<HRAdmin> Get(int key)
        {
            return await _notifyContext.HRAdmins.SingleOrDefaultAsync(u => u.Id == key);
        }

        public override async Task<IEnumerable<HRAdmin>> GetAll()
        {
            return await _notifyContext.HRAdmins.ToListAsync();
        }
            
    }
}