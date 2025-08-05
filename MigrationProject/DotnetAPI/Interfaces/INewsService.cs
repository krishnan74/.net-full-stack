using System.Threading.Tasks;
using DotnetAPI.Models;
using DotnetAPI.DTOs.News;

namespace DotnetAPI.Interfaces
{
    public interface INewsService
    {
        Task<News> AddNewsAsync(AddNewsDTO news, string filePath = null);
        Task<News> UpdateNewsAsync(int id, UpdateNewsDTO news, string filePath = null);
        Task<News> DeleteNewsAsync(int id);
        Task<News> GetNewsByIdAsync(int id);
        Task<IEnumerable<News>> GetAllNewsAsync(int pageNumber, int pageSize);
        Task<string> ExportNewsToCSVAsync();
        Task<string> ExportNewsToExcelAsync();
    }
}
