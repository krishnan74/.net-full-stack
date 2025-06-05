using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Notify.Contexts;
using Notify.Interfaces;
using Notify.Models;
using System.Security.Claims;

namespace Notify.Services;

public class FileService : IFileService
{
    private readonly NotifyContext _context;
    private readonly ILogger<FileService> _logger;
    private readonly string _uploadPath;

    public FileService(NotifyContext context, ILogger<FileService> logger, IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _uploadPath = configuration["FileStorage:BasePath"] ?? "uploads";

        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public async Task<Document> UploadDocumentAsync(IFormFile file, string fileType, string userId)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file uploaded");

        var allowedExtensions = (fileType ?? "").Split(',');
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtension))
            throw new ArgumentException("Invalid file type");

        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(_uploadPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var document = new Document
        {
            Title = Path.GetFileNameWithoutExtension(file.FileName),
            FileName = fileName,
            FilePath = filePath,
            ContentType = file.ContentType,
            FileSize = file.Length,
            UploadedById = userId,
            UploadedAt = DateTime.UtcNow
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return document;
    }

    public async Task<IEnumerable<Document>> GetAccessibleDocumentsAsync(string userId)
    {
        return await _context.Documents
            .Include(d => d.DocumentAccesses)
            .Where(d => d.UploadedById == userId ||
                       d.DocumentAccesses.Any(da => da.UserId.ToString() == userId &&
                                                  (!da.ExpiresAt.HasValue || da.ExpiresAt > DateTime.UtcNow)))
            .ToListAsync();
    }

    public async Task<(Stream fileStream, string fileName, string contentType)> DownloadDocumentAsync(int documentId, string userId)
    {
        var document = await _context.Documents
            .Include(d => d.DocumentAccesses)
            .FirstOrDefaultAsync(d => d.Id == documentId);

        if (document == null)
            throw new KeyNotFoundException("Document not found");

        if (document.UploadedById != userId &&
            !document.DocumentAccesses.Any(da =>
                da.UserId.ToString() == userId &&
                da.CanDownload &&
                (!da.ExpiresAt.HasValue || da.ExpiresAt > DateTime.UtcNow)))
        {
            throw new UnauthorizedAccessException("You don't have permission to download this document");
        }

        if (!File.Exists(document.FilePath))
            throw new FileNotFoundException("Document file not found");

        var fileStream = new FileStream(document.FilePath, FileMode.Open, FileAccess.Read);
        return (fileStream, document.Title + Path.GetExtension(document.FileName), document.ContentType);
    }

    public async Task GrantDocumentAccessAsync(int documentId, int userId, string grantedBy, bool canView = true, bool canDownload = true)
    {
        var document = await _context.Documents.FindAsync(documentId);
        if (document == null)
            throw new KeyNotFoundException("Document not found");

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        var existingAccess = await _context.DocumentAccesses
            .FirstOrDefaultAsync(da => da.DocumentId == documentId && da.UserId == userId);

        if (existingAccess != null)
        {
            existingAccess.CanView = canView;
            existingAccess.CanDownload = canDownload;
            existingAccess.GrantedBy = grantedBy;
            existingAccess.GrantedAt = DateTime.UtcNow;
        }
        else
        {
            var documentAccess = new DocumentAccess
            {
                DocumentId = documentId,
                UserId = userId,
                GrantedBy = grantedBy,
                CanView = canView,
                CanDownload = canDownload
            };
            _context.DocumentAccesses.Add(documentAccess);
        }

        await _context.SaveChangesAsync();
    }

    public async Task PostFileAsync(IFormFile file, string fileType)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file uploaded");

        var fileDetails = new FileDetails
        {
            FileName = file.FileName,
            FileType = fileType,
            FileData = new byte[file.Length]
        };

        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            fileDetails.FileData = stream.ToArray();
        }

        _context.FileDetails.Add(fileDetails);
        await _context.SaveChangesAsync();
    }

    public async Task PostMultiFileAsync(List<FileUploadModel> fileDetails)
    {
        foreach (var file in fileDetails)
        {
            await PostFileAsync(file.FileDetails, file.FileType);
        }
    }

    public async Task DownloadFileById(int id)
    {
        var fileDetails = await _context.FileDetails.FindAsync(id);
        if (fileDetails == null)
            throw new KeyNotFoundException("File not found");

        try
        {
            var content = new System.IO.MemoryStream(fileDetails.FileData);
            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "FileDownloaded",
                fileDetails.FileName);

            await CopyStream(content, path);
        }
        catch (Exception)
        {
            throw;
        }
        _logger.LogInformation("File {FileName} downloaded successfully", fileDetails.FileName);

    }
    
    public async Task CopyStream(Stream stream, string downloadPath)
    {
        using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
        {
        await stream.CopyToAsync(fileStream);
        }
    }
}