using System.ComponentModel.DataAnnotations;

namespace Notify.Models;

public class Document
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;    
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string UploadedById { get; set; } = string.Empty;
    public User UploadedBy { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; }
    public ICollection<DocumentAccess> DocumentAccesses { get; set; } = new List<DocumentAccess>();
} 