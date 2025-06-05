namespace FileAPI.Models
{
    public class FileUploadModel
    {
        public IFormFile FileDetails { get; set; }
        public string FileType { get; set; }
    }
}