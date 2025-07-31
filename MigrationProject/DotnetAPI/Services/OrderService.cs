using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DotnetAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly DotnetAPIContext _context;
        public OrderService(DotnetAPIContext context)
        {
            _context = context;
        }

        public async Task<byte[]> ExportOrderListingPdfAsync()
        {
            // Placeholder: In a real app, use a PDF library (e.g., iTextSharp, QuestPDF, etc.)
            // Here, we just export as plain text for demonstration
            var orders = await _context.Orders.OrderByDescending(x => x.OrderID).ToListAsync();
            var sb = new StringBuilder();
            sb.AppendLine("OrderID,OrderName,OrderDate,PaymentType,Status,CustomerName,CustomerPhone,CustomerEmail,CustomerAddress");
            foreach (var order in orders)
            {
                sb.AppendLine($"{order.OrderID},{order.OrderName},{order.OrderDate},{order.PaymentType},{order.Status},{order.CustomerName},{order.CustomerPhone},{order.CustomerEmail},{order.CustomerAddress}");
            }
            // Convert to bytes (simulate PDF)
            return Encoding.UTF8.GetBytes(sb.ToString());
        }
        // ...CRUD and other methods as needed...
    }
}
