using AzureKeyVaultDemo.Models;

namespace AzureKeyVaultDemo.Interfaces
{
    public interface IFileService
    {
        public Task PostFileAsync(IFormFile file);

        public Task PostMultiFileAsync(List<IFormFile> fileData);

        public  Task<Stream> DownloadFile(string fileName);

        public Task DeleteFileAsync(string fileName);
    }
}