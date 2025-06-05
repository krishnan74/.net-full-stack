namespace Notify.Models.DTOs;

public class ErrorObjectDTO
{
    public int ErrorNumber { get; set; }
    public required string ErrorMessage { get; set; }
    public int StatusCode { get; set; }
}