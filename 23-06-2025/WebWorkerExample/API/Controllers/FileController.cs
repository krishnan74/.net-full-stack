using System.Data;
using API.Interfaces;
using API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileProcessingService _processingService;

    public FileController(IFileProcessingService processingService)
    {

        _processingService = processingService;
    }

    [HttpPost("FromCsv")]
    public async Task<IActionResult> BulkInsertFromCsv([FromBody] CsvUploadDTO input)
    {
        return Ok(await _processingService.ProcessData(input));
    }
}