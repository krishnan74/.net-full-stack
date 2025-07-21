namespace VideoPortalAPI.Models.DTOs.TrainingVideo
{
    public class TrainingVideoAddRequestDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile File { get; set; }
    }

    public class TrainingVideoUpdateRequestDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}