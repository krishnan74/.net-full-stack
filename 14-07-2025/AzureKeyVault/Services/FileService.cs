using AzureKeyVaultDemo.Models;
using AzureKeyVaultDemo.Interfaces;
using Azure.Storage.Blobs;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace AzureKeyVaultDemo.Services
{
    public class FileService : IFileService
    {
        private readonly SecretClient _secretClient;

        public  FileService(IConfiguration configuration)
        {
            var keyVaultUrl = configuration.GetValue<string>("AzureBlob:KeyVaultUrl");
            _secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

            
        }
 
        public async Task PostFileAsync(IFormFile file)
        {
            try
            {
                KeyVaultSecret blobStorageConnectionVault = await _secretClient.GetSecretAsync("connection-string");
                KeyVaultSecret containerNameVault = await _secretClient.GetSecretAsync("container-name");

                Console.WriteLine($"Blob Storage Connection : {blobStorageConnectionVault.Value}");
                Console.WriteLine($"Blob Container Name: {containerNameVault.Value}");

                var blobStorageConnectionString = blobStorageConnectionVault.Value?.ToString();
                var containerName = containerNameVault.Value?.ToString();

                Console.WriteLine($"Blob Storage Connection String: {blobStorageConnectionString}");
                Console.WriteLine($"Blob Container Name String: {containerName}"); 

                var containerClient = new BlobContainerClient(blobStorageConnectionString, containerName);
                containerClient.CreateIfNotExists();

                var blobClient = containerClient.GetBlobClient(file.FileName);
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
                var blobStorageConnectionVault = await _secretClient.GetSecretAsync("connection-string");
                var containerNameVault = await _secretClient.GetSecretAsync("container-name");

                 var blobStorageConnectionString = blobStorageConnectionVault.Value?.ToString();
                var containerName = containerNameVault.Value?.ToString();

                var containerClient = new BlobContainerClient(blobStorageConnectionString, containerName);
                containerClient.CreateIfNotExists();

                foreach (var file in fileData)
                {
                    var blobClient = containerClient.GetBlobClient(file.FileName);
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
                var blobStorageConnectionVault = await _secretClient.GetSecretAsync("connection-string");
                var containerNameVault = await _secretClient.GetSecretAsync("container-name");

                 var blobStorageConnectionString = blobStorageConnectionVault.Value?.ToString();
                var containerName = containerNameVault.Value?.ToString();

                var containerClient = new BlobContainerClient(blobStorageConnectionString, containerName);
                containerClient.CreateIfNotExists();

                var blobClient = containerClient.GetBlobClient(fileName);
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
                var blobStorageConnectionVault = await _secretClient.GetSecretAsync("connection-string");
                var containerNameVault = await _secretClient.GetSecretAsync("container-name");

                 var blobStorageConnectionString = blobStorageConnectionVault.Value?.ToString();
                var containerName = containerNameVault.Value?.ToString();

                var containerClient = new BlobContainerClient(blobStorageConnectionString, containerName);
                containerClient.CreateIfNotExists();

                var blobClient = containerClient.GetBlobClient(fileName);
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