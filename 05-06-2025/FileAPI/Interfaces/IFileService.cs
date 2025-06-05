using FileAPI.Models;

namespace FileAPI.Interfaces
{
    public interface IFileService
    {
        public Task PostFileAsync(IFormFile fileData, string fileType);

        public Task PostMultiFileAsync(List<FileUploadModel> fileData);

        public Task DownloadFileById(int fileName);
    }
}