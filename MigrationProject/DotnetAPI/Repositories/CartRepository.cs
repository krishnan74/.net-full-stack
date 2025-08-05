using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.Repositories
{
    public class CartRepository : Repository<int, Cart>
    {
        public CartRepository(DotnetAPIContext context) : base(context) { }

        public override async Task<Cart> Get(int key)
        {
            var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.CartId == key);
            return cart ?? throw new KeyNotFoundException($"No cart with the given ID: {key}");
        }

        public override async Task<IEnumerable<Cart>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var carts = _context.Carts.Include(c => c.Items);
            if (!carts.Any())
                return Enumerable.Empty<Cart>();
            return await carts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
