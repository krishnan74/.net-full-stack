using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.Repositories
{
    public class ModelRepository : Repository<int, Model>
    {
        public ModelRepository(DotnetAPIContext context) : base(context) { }

        public override async Task<Model> Get(int key)
        {
            var model = await _context.Models.FirstOrDefaultAsync(c => c.ModelId == key);
            return model ?? throw new KeyNotFoundException($"No model with the given ID: {key}");
        }

        public override async Task<IEnumerable<Model>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var models = _context.Models;
            if (!models.Any())
                return Enumerable.Empty<Model>();
            return await models.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
