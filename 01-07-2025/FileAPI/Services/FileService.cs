using FileAPI.Models;
using FileAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileAPI.Services
{
    public class FileService : IFileService
    {
        private readonly IRepository<int, FileDetails> _fileRepository;
        public FileService(IRepository<int, FileDetails> fileRepository)
        {
            _fileRepository = fileRepository;
        }
      
        public async Task PostFileAsync(IFormFile fileData, string fileType)
        {
            try
            {
                var fileDetails = new FileDetails()
                {
                    FileName = fileData.FileName,
                    FileType = fileType,
                };

                using (var stream = new MemoryStream())
                {
                    fileData.CopyTo(stream);
                    fileDetails.FileData = stream.ToArray();
                }

                var result = await _fileRepository.Add(fileDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task PostMultiFileAsync(List<FileUploadModel> fileData)
        {
            try
            {
                foreach(FileUploadModel file in fileData)
                {
                    var fileDetails = new FileDetails()
                    {
                        FileName = file.FileDetails.FileName,
                        FileType = file.FileType,
                    };

                    using (var stream = new MemoryStream())
                    {
                        file.FileDetails.CopyTo(stream);
                        fileDetails.FileData = stream.ToArray();
                    }

                    var result = await _fileRepository.Add(fileDetails);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DownloadFileById(int Id)
        {
            try
            {
                var file =  await _fileRepository.Get(Id);

                var content = new System.IO.MemoryStream(file.FileData);
                var path = Path.Combine(
                   Directory.GetCurrentDirectory(), "FileDownloaded",
                   file.FileName);

                await CopyStream(content, path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CopyStream(Stream stream, string downloadPath)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
            {
               await stream.CopyToAsync(fileStream);
            }
        }
    }
}