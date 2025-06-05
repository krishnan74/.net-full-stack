using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Notify.Models
{
    public class FileUploadModel
    {
        [Required]
        public required IFormFile FileDetails { get; set; }
        
        [Required]
        public required string FileType { get; set; }
    }
}