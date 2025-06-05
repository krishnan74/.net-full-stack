using System.ComponentModel.DataAnnotations;

namespace Notify.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public byte[]? Password { get; set; }
        public byte[]? HashKey { get; set; }
        public HRAdmin? HRAdmin { get; set; }
        public Employee? Employee { get; set; }
        public ICollection<Document>? UploadedDocuments { get; set; } = new List<Document>();
        public ICollection<DocumentAccess>? DocumentAccesses { get; set; } = new List<DocumentAccess>();
    }
}