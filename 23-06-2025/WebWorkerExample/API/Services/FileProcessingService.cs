using System.Data;
using API.Contexts;
using API.Interfaces;
using API.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        public readonly FileContext _context;
        public FileProcessingService(FileContext context)
        {
            _context = context;
        }

        public async Task<FileUploadReturnDTO> ProcessData(CsvUploadDTO csvUploadDto)
        {
            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM process_csv(:csv_input)";

            command.CommandType = CommandType.Text;

            var param = command.CreateParameter();
            param.ParameterName = "csv_input";
            param.Value = csvUploadDto.CsvContent;
            command.Parameters.Add(param);

            using var reader = await command.ExecuteReaderAsync();

            var errorRows = new List<string>();
            while (await reader.ReadAsync())
            {
                errorRows.Add(reader.GetString(0));
            }
            return new FileUploadReturnDTO{ Inserted = "CSV Processed", Errors = errorRows.ToArray() };
        }
    }
}