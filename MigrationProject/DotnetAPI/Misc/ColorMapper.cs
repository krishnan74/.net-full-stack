using DotnetAPI.Models;
using DotnetAPI.DTOs.Color;

namespace DotnetAPI.Misc.Mappers
{
    public class ColorMapper
    {
        public Color? MapAddColor(AddColorDTO addRequestDto)
        {
            if (addRequestDto == null)
                return null;
            Color color = new();
            color.Color1 = addRequestDto.Color1;
            return color;
        }

        public Color? MapUpdateColor(Color existingColor, UpdateColorDTO updateRequestDto)
        {
            if (existingColor == null || updateRequestDto == null)
                return null;
            existingColor.Color1 = updateRequestDto.Color1;
            return existingColor;
        }
    }
}
