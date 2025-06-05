using System.ComponentModel.DataAnnotations;

namespace Notify.Models;

public class DocumentAccess
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public Document Document { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } 
    public DateTime GrantedAt { get; set; } = DateTime.Now;
    public string GrantedBy { get; set; } = string.Empty;
    public bool CanView { get; set; } = true;
    public bool CanDownload { get; set; } = true;
    public DateTime? ExpiresAt { get; set; }
} 