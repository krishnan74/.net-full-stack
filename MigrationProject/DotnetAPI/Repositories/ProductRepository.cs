using DotnetAPI.Models;
using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Repositories
{
    public class ProductRepository : Repository<int, Product>
    {
        public ProductRepository(DotnetAPIContext context) : base(context) { }

        public override async Task<Product> Get(int key)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == key);
            return product ?? throw new KeyNotFoundException($"No product with the given ID: {key}");
        }

        public override async Task<IEnumerable<Product>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var products = _context.Products;
            if (!products.Any())
                return Enumerable.Empty<Product>();
            return await products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
