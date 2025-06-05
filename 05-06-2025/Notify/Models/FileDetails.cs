using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notify.Models
{
    public class FileDetails
    {
        public int Id { get; set; }
        
        [Required]
        public required string FileName { get; set; }
        
        [Required]
        public required byte[] FileData { get; set; }
        
        [Required]
        public required string FileType { get; set; }
    }
}