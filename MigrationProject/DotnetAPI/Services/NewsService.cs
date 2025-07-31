using System.Text;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.Contexts;
using DotnetAPI.DTOs.News;
using DotnetAPI.Misc.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Services
{
    public class NewsService : INewsService
    {
        private readonly IRepository<int, News> _newsRepository;
        public NewsMapper newsMapper;
        private readonly DotnetAPIContext _context;
        public NewsService(IRepository<int, News> newsRepository, DotnetAPIContext context)
        {
            _newsRepository = newsRepository;
            newsMapper = new NewsMapper();
            _context = context;
        }

        public async Task<News> AddNewsAsync(AddNewsDTO newsDTO)
        {
            if (newsDTO == null)
                throw new ArgumentNullException(nameof(newsDTO));
            if (string.IsNullOrWhiteSpace(newsDTO.Title))
                throw new ArgumentException("Title cannot be null or empty.");
            var newNews = newsMapper.MapAddNews(newsDTO);
            if (newNews == null)
                throw new Exception("Failed to map AddNewsDTO to News.");
            var addedNews = await _newsRepository.Add(newNews);
            if (addedNews == null)
                throw new Exception("Failed to add new news.");
            return addedNews;
        }

        public async Task<News> UpdateNewsAsync(int id, UpdateNewsDTO newsDTO)
        {
            if (newsDTO == null)
                throw new ArgumentNullException(nameof(newsDTO));
            var news = await _newsRepository.Get(id);
            if (news == null)
                throw new KeyNotFoundException($"No news with the given ID: {id}");
            var updatedNews = newsMapper.MapUpdateNews(news, newsDTO);
            if (updatedNews == null)
                throw new Exception("Failed to map UpdateNewsDTO to News.");
            var result = await _newsRepository.Update(id, updatedNews);
            if (result == null)
                throw new Exception("Failed to update news.");
            return result;
        }

        public async Task<News> DeleteNewsAsync(int id)
        {
            var deletedNews = await _newsRepository.Delete(id);
            return deletedNews;
        }

        public async Task<News> GetNewsByIdAsync(int id)
        {
            return await _newsRepository.Get(id);
        }

        public async Task<IEnumerable<News>> GetAllNewsAsync(int pageNumber, int pageSize)
        {
            return await _newsRepository.GetAll(pageNumber, pageSize);
        }

        public async Task<string> ExportNewsToCSVAsync()
        {
            var newsList = await _context.News.OrderBy(x => x.NewsId).ToListAsync();
            var sb = new StringBuilder();
            sb.AppendLine("\"NewsId\",\"Title\",\"ShortDescription\",\"CreatedDate\",\"Status\"");
            foreach (var news in newsList)
            {
                sb.AppendLine($"\"{news.NewsId}\",\"{news.Title}\",\"{news.ShortDescription}\",\"{news.CreatedDate}\",\"{news.Status}\"");
            }
            return sb.ToString();
        }

        public async Task<string> ExportNewsToExcelAsync()
        {
            var newsList = await _context.News.OrderBy(x => x.NewsId).ToListAsync();
            var sb = new StringBuilder();
            sb.AppendLine("<table border='1'><tr><th>NewsId</th><th>Title</th><th>ShortDescription</th><th>CreatedDate</th><th>Status</th></tr>");
            foreach (var news in newsList)
            {
                sb.AppendLine($"<tr><td>{news.NewsId}</td><td>{news.Title}</td><td>{news.ShortDescription}</td><td>{news.CreatedDate}</td><td>{news.Status}</td></tr>");
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }
    }
}
