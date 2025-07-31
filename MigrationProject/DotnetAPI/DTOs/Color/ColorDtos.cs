namespace DotnetAPI.DTOs.Color
{
    public class ColorDto
    {
        public int ColorId { get; set; }
        public string Color1 { get; set; }
    }

    public class CreateColorDto
    {
        public string Color1 { get; set; }
    }

    public class UpdateColorDto
    {
        public int ColorId { get; set; }
        public string Color1 { get; set; }
    }

    public class AddColorDTO
    {
        public string Color1 { get; set; }
    }
    public class UpdateColorDTO
    {
        public string Color1 { get; set; }
    }
}
