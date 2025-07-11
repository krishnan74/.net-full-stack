using AzureBlobStorageDemo.Models;
using AzureBlobStorageDemo.Interfaces;
using Azure.Storage.Blobs;

namespace AzureBlobStorageDemo.Services
{
    public class FileService : IFileService
    {
        private readonly BlobContainerClient _containerClient;

        public FileService(IConfiguration configuration)
        {
            var blobStorageConnection = configuration.GetValue<string>("AzureBlob:ConnectionString");
            var containerName = configuration.GetValue<string>("AzureBlob:ContainerName");
            _containerClient = new BlobContainerClient(blobStorageConnection, containerName);
            _containerClient.CreateIfNotExists();
        }
 
        public async Task PostFileAsync(IFormFile file)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(file.FileName);
                await blobClient.UploadAsync(file.OpenReadStream(), true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task PostMultiFileAsync(List<IFormFile> fileData)
        {
            try
            {
                foreach (var file in fileData)
                {
                    var blobClient = _containerClient.GetBlobClient(file.FileName);
                    await blobClient.UploadAsync(file.OpenReadStream(), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Stream> DownloadFile(string fileName)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(fileName);
                var memoryStream = new MemoryStream();

                if (await blobClient.ExistsAsync())
                {
                    await blobClient.DownloadToAsync(memoryStream);
                    memoryStream.Position = 0;
                }

                return memoryStream;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteFileAsync(string fileName)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(fileName);
                if (await blobClient.ExistsAsync())
                {
                    await blobClient.DeleteAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}