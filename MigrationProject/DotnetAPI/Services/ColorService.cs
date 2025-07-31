using System.Collections.Generic;
using System.Threading.Tasks;
using DotnetAPI.Models;
using DotnetAPI.Interfaces;
using DotnetAPI.Contexts;
using DotnetAPI.DTOs.Color;
using DotnetAPI.Misc.Mappers;

namespace DotnetAPI.Services
{
    public class ColorService : IColorService
    {
        private readonly IRepository<int, Color> _colorRepository;
        public ColorMapper colorMapper;
        public ColorService(IRepository<int, Color> colorRepository)
        {
            _colorRepository = colorRepository;
            colorMapper = new ColorMapper();
        }

        public async Task<Color> AddColorAsync(AddColorDTO colorDTO)
        {
            if (colorDTO == null)
                throw new ArgumentNullException(nameof(colorDTO));
            if (string.IsNullOrWhiteSpace(colorDTO.Color1))
                throw new ArgumentException("Color name cannot be null or empty.");
            var newColor = colorMapper.MapAddColor(colorDTO);
            if (newColor == null)
                throw new Exception("Failed to map AddColorDTO to Color.");
            var addedColor = await _colorRepository.Add(newColor);
            if (addedColor == null)
                throw new Exception("Failed to add new color.");
            return addedColor;
        }

        public async Task<Color> UpdateColorAsync(int id, UpdateColorDTO colorDTO)
        {
            if (colorDTO == null)
                throw new ArgumentNullException(nameof(colorDTO));
            var color = await _colorRepository.Get(id);
            if (color == null)
                throw new KeyNotFoundException($"No color with the given ID: {id}");
            var updatedColor = colorMapper.MapUpdateColor(color, colorDTO);
            if (updatedColor == null)
                throw new Exception("Failed to map UpdateColorDTO to Color.");
            var result = await _colorRepository.Update(id, updatedColor);
            if (result == null)
                throw new Exception("Failed to update color.");
            return result;
        }

        public async Task<Color> DeleteColorAsync(int id)
        {
            var deletedColor = await _colorRepository.Delete(id);
            return deletedColor;
        }

        public async Task<Color> GetColorByIdAsync(int id)
        {
            return await _colorRepository.Get(id);
        }

        public async Task<IEnumerable<Color>> GetAllColorsAsync(int pageNumber, int pageSize)
        {
            return await _colorRepository.GetAll(pageNumber, pageSize);
        }
    }
}
