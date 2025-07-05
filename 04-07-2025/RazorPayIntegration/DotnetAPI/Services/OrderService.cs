using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.Contexts;
using DotnetAPI.Models.DTOs.Order;

namespace DotnetAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Guid, Order> _orderRepository;
        private readonly IRazorpayService _razorpayService;
        private readonly DatabaseContext _context;
        public OrderService(IRepository<Guid, Order> orderRepository, DatabaseContext context, IRazorpayService razorpayService)
        {
            _orderRepository = orderRepository;
            _context = context;
            _razorpayService = razorpayService;
        }

        public async Task<Order> CreateOrderAsync(OrderDTO orderDTO)
        {
            try
            {
                Console.WriteLine("Validating orderDTO fields...");
                if (string.IsNullOrWhiteSpace(orderDTO.CustomerName) || string.IsNullOrWhiteSpace(orderDTO.Email) || string.IsNullOrWhiteSpace(orderDTO.ContactNumber))
                {
                    Console.WriteLine("Validation failed: Missing required fields.");
                    throw new ArgumentException("Order customer name, email, and contact number cannot be null or empty.");
                }

                var razorpayOrder = _razorpayService.CreateOrder(orderDTO.Amount * 100);
                if (razorpayOrder == null)
                {
                    Console.WriteLine("Failed to create Razorpay order.");
                    throw new Exception("Failed to create Razorpay order.");
                }

                Console.WriteLine("Mapping OrderDTO to Order entity...");
                var newOrder = new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerName = orderDTO.CustomerName,
                    Email = orderDTO.Email,
                    ContactNumber = orderDTO.ContactNumber,
                    RazorpayOrderId = razorpayOrder["id"].ToString(),
                };

                if (newOrder == null)
                {
                    Console.WriteLine("Mapping failed: newOrder is null.");
                    throw new Exception("Failed to map OrderDTO to Order.");
                }

                Console.WriteLine("Adding new order to repository...");
                var addedOrder = await _orderRepository.Add(newOrder);

                if (addedOrder == null)
                {
                    Console.WriteLine("Failed to add new order to repository.");
                    throw new Exception("Failed to add new order.");
                }

                Console.WriteLine("Order added successfully.");
                return addedOrder;

            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Invalid order data: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the order.", ex);
            }
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            try
            {
                return await _orderRepository.Get(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the order with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
             try
            {
                return await _orderRepository.GetAll();

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all orders.", ex);
            }
        }
    
    }
}