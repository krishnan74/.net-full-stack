using System.Threading.Tasks;
using DotnetAPI.Models;
using System.Collections.Generic;

namespace DotnetAPI.Interfaces
{
    public interface IOrderService
    {
        Task<byte[]> ExportOrderListingPdfAsync();
        // ...CRUD and other methods as needed...
    }
}
