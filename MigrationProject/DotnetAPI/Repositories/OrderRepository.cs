using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.Repositories
{
    public class OrderRepository : Repository<int, Order>
    {
        public OrderRepository(DotnetAPIContext context) : base(context) { }

        public override async Task<Order> Get(int key)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.OrderID == key);
            return order ?? throw new KeyNotFoundException($"No order with the given ID: {key}");
        }

        public override async Task<IEnumerable<Order>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var orders = _context.Orders.Include(o => o.OrderDetails);
            if (!orders.Any())
                return Enumerable.Empty<Order>();
            return await orders.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
