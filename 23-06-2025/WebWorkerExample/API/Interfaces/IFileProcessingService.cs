using API.Models.DTOs;

namespace API.Interfaces
{
    public interface IFileProcessingService
    {
        public Task<FileUploadReturnDTO> ProcessData(CsvUploadDTO csvUploadDto);
    }
}