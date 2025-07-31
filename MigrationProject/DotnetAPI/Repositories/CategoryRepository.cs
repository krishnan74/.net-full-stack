using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Repositories
{
    public class CategoryRepository : Repository<int, Category>
    {
        public CategoryRepository(DotnetAPIContext context) : base(context)
        {
        }

        public override async Task<Category> Get(int key)
        {
            var category = await _context.Categories
                .SingleOrDefaultAsync(p => p.CategoryId == key);

            return category ?? throw new KeyNotFoundException($"No category with the given ID: {key}");
        }

        public override async Task<IEnumerable<Category>> GetAll(
            int pageNumber = 1,
            int pageSize = 10
        )
        {
            var categories = _context.Categories;
            if (!categories.Any())
                return Enumerable.Empty<Category>();

            return await categories.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

    }
}