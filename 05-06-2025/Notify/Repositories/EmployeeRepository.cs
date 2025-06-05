using Notify.Contexts;
using Notify.Interfaces;
using Notify.Models;
using Microsoft.EntityFrameworkCore;

namespace Notify.Repositories
{
    public class EmployeeRepository : Repository<int, Employee>
    {
        public EmployeeRepository(NotifyContext notifyContext):base(notifyContext)
        {
            
        }
        public override async Task<Employee> Get(int key)
        {
            return await _notifyContext.Employees.SingleOrDefaultAsync(u => u.Id == key);
        }

        public override async Task<IEnumerable<Employee>> GetAll()
        {
            return await _notifyContext.Employees.ToListAsync();
        }
            
    }
}