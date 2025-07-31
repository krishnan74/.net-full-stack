using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Repositories
{
    public class NewsRepository : Repository<int, News>
    {
        public NewsRepository(DotnetAPIContext context) : base(context) { }

        public override async Task<News> Get(int key)
        {
            var news = await _context.News.SingleOrDefaultAsync(n => n.NewsId == key);
            return news ?? throw new KeyNotFoundException($"No news with the given ID: {key}");
        }

        public override async Task<IEnumerable<News>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var news = _context.News;
            if (!news.Any())
                return Enumerable.Empty<News>();
            return await news.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
