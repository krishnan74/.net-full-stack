using System.Collections.Generic;
using System.Threading.Tasks;
using DotnetAPI.Models;
using DotnetAPI.DTOs.Color;

namespace DotnetAPI.Interfaces
{
    public interface IColorService
    {
        Task<Color> AddColorAsync(AddColorDTO color);
        Task<Color> UpdateColorAsync(int id, UpdateColorDTO color);
        Task<Color> DeleteColorAsync(int id);
        Task<Color> GetColorByIdAsync(int id);
        Task<IEnumerable<Color>> GetAllColorsAsync(int pageNumber, int pageSize);
    }
}
