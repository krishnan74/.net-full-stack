using Microsoft.AspNetCore.Http;
using Notify.Models;

namespace Notify.Interfaces
{
    public interface IFileService
    {
        Task<Document> UploadDocumentAsync(IFormFile file, string fileType, string userId);
        Task<IEnumerable<Document>> GetAccessibleDocumentsAsync(string userId);
        Task<(Stream fileStream, string fileName, string contentType)> DownloadDocumentAsync(int documentId, string userId);
        Task GrantDocumentAccessAsync(int documentId, int userId, string grantedBy, bool canView = true, bool canDownload = true);
    }
}