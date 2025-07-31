using DotnetAPI.Models;
using DotnetAPI.DTOs.News;

namespace DotnetAPI.Misc.Mappers
{
    public class NewsMapper
    {
        public News? MapAddNews(AddNewsDTO addRequestDto)
        {
            if (addRequestDto == null)
                return null;
            News news = new();
            news.UserId = addRequestDto.UserId;
            news.Title = addRequestDto.Title;
            news.ShortDescription = addRequestDto.ShortDescription;
            news.Image = addRequestDto.Image;
            news.Content = addRequestDto.Content;
            news.CreatedDate = addRequestDto.CreatedDate;
            news.Status = addRequestDto.Status;
            return news;
        }

        public News? MapUpdateNews(News existingNews, UpdateNewsDTO updateRequestDto)
        {
            if (existingNews == null || updateRequestDto == null)
                return null;
            existingNews.Title = updateRequestDto.Title;
            existingNews.ShortDescription = updateRequestDto.ShortDescription;
            existingNews.Image = updateRequestDto.Image;
            existingNews.Content = updateRequestDto.Content;
            existingNews.CreatedDate = updateRequestDto.CreatedDate;
            existingNews.Status = updateRequestDto.Status;
            return existingNews;
        }
    }
}
