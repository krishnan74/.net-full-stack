using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Repositories
{
    public  class OrderRepository : Repository<Guid, Order>
    {
        public OrderRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public override async Task<Order> Get(Guid key)
        {
            var order = await _databaseContext.orders.Include(o => o.Payment).SingleOrDefaultAsync(p => p.Id == key);

            return order??throw new Exception("No order with the given ID");
        }

        public override async Task<IEnumerable<Order>> GetAll()
        {
            var orders = _databaseContext.orders.Include(o => o.Payment);
            if (!orders.Any())
                return Enumerable.Empty<Order>();
            return (await orders.ToListAsync());
        }
    }
}