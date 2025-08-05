using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using DotnetAPI.DTOs.Order;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using QuestPDF.Elements;

namespace DotnetAPI.Services
{
    public partial class OrderService : IOrderService
    {
        private readonly DotnetAPIContext _context;
        private readonly IRepository<int, Order> _orderRepository;
        public OrderService(DotnetAPIContext context, IRepository<int, Order> orderRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
        }

        public async Task<byte[]> ExportOrderListingPdfAsync()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var orders = await _context.Orders
                .AsNoTracking()
                .OrderByDescending(x => x.OrderID)
                .ToListAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);
                    page.Size(PageSizes.A3);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text("Order Listing")
                        .FontSize(16)
                        .Bold()
                        .AlignCenter();

                    // Content
                    page.Content().Table(table =>
                    {
                        
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1); // OrderID
                                columns.RelativeColumn(2);    // OrderName
                                columns.RelativeColumn(1.5f);    // OrderDate
                                columns.RelativeColumn(1);    // PaymentType
                                columns.RelativeColumn(1);    // Status
                                columns.RelativeColumn(2);    // CustomerName
                                columns.RelativeColumn(2);    // CustomerPhone
                                columns.RelativeColumn(2);    // CustomerEmail
                                columns.RelativeColumn(2); // CustomerAddress
                            });

                            string[] headers = {
                                "OrderID", "OrderName", "OrderDate", "PaymentType", "Status",
                                "CustomerName", "CustomerPhone", "CustomerEmail", "CustomerAddress"
                            };



                            // Header Row
                            table.Header(header =>
                            {

                                void HeaderCell(string text) =>
                                header.Cell().Element(CellStyle).Text(text).Bold();

                                HeaderCell("OrderID");
                                HeaderCell("OrderName");
                                HeaderCell("OrderDate");
                                HeaderCell("PaymentType");
                                HeaderCell("Status");
                                HeaderCell("CustomerName");
                                HeaderCell("CustomerPhone");
                                HeaderCell("CustomerEmail");
                                HeaderCell("CustomerAddress");
                            });

                            // Data Rows
                            int rowIndex = 0;
                            foreach (var order in orders)
                            {
                                bool isEven = rowIndex++ % 2 == 0;
                                var bgColor = isEven ? Colors.Grey.Lighten3 : Colors.White;

                                void DataCell(string text) => table.Cell().Element(CellStyle).Text(text);


                                DataCell(order.OrderID.ToString());
                                DataCell(order.OrderName);
                                DataCell(order.OrderDate?.ToString("yyyy-MM-dd") ?? "N/A");
                                DataCell(order.PaymentType);
                                DataCell(order.Status);
                                DataCell(order.CustomerName);
                                DataCell(order.CustomerPhone);
                                DataCell(order.CustomerEmail);
                                DataCell(order.CustomerAddress);

                                
                            }

                            static IContainer CellStyle(IContainer container)
                            {
                                return container
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Padding(4);
                            }
                    });

                    page.Footer().AlignCenter().Text($"Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                        .FontSize(10)
                        .Italic();
                });
            });

            using var ms = new MemoryStream();
            document.GeneratePdf(ms);
            return ms.ToArray();
        }



        public async Task<Order> AddOrderAsync(AddOrderDTO orderDTO)
        {
            var order = new Order
            {
                OrderName = orderDTO.OrderName,
                OrderDate = orderDTO.OrderDate,
                PaymentType = orderDTO.PaymentType,
                Status = orderDTO.Status,
                CustomerName = orderDTO.CustomerName,
                CustomerPhone = orderDTO.CustomerPhone,
                CustomerEmail = orderDTO.CustomerEmail,
                CustomerAddress = orderDTO.CustomerAddress
            };
            return await _orderRepository.Add(order);
        }

        public async Task<Order> UpdateOrderAsync(int id, UpdateOrderDTO orderDTO)
        {
            var order = await _orderRepository.Get(id);
            if (order == null)
                throw new KeyNotFoundException($"No order with the given ID: {id}");
            order.OrderName = orderDTO.OrderName;
            order.OrderDate = orderDTO.OrderDate;
            order.PaymentType = orderDTO.PaymentType;
            order.Status = orderDTO.Status;
            order.CustomerName = orderDTO.CustomerName;
            order.CustomerPhone = orderDTO.CustomerPhone;
            order.CustomerEmail = orderDTO.CustomerEmail;
            order.CustomerAddress = orderDTO.CustomerAddress;
            return await _orderRepository.Update(id, order);
        }

        public async Task<Order> DeleteOrderAsync(int id)
        {
            return await _orderRepository.Delete(id);
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.Get(id);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync(int pageNumber, int pageSize)
        {
            return await _orderRepository.GetAll(pageNumber, pageSize);
        }
    }
}
