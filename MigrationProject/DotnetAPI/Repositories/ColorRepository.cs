using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Models;
using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;

namespace DotnetAPI.Repositories
{
    public class ColorRepository : Repository<int, Color>
    {
        public ColorRepository(DotnetAPIContext context) : base(context) { }

        public override async Task<Color> Get(int key)
        {
            var color = await _context.Colors.SingleOrDefaultAsync(c => c.ColorId == key);
            return color ?? throw new KeyNotFoundException($"No color with the given ID: {key}");
        }

        public override async Task<IEnumerable<Color>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var colors = _context.Colors;
            if (!colors.Any())
                return Enumerable.Empty<Color>();
            return await colors.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
