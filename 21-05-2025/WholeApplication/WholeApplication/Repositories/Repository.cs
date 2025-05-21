using System;
using System.Collections.Generic;
using System.Linq;

namespace WholeApplication.Repositories
{
    // Base interface for entities
    public interface IEntity
    {
        int Id { get; set; }
    }

    // Sample entity class
    public class Product : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Price: ₹{Price:F2}, Category: {Category}";
        }
    }

    // Generic repository interface
    public interface IRepository<T> where T : class, IEntity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        bool Exists(int id);
    }

    // Generic repository implementation
    public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly List<T> _entities = new List<T>();
        private int _nextId = 1;

        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        public T GetById(int id)
        {
            return _entities.FirstOrDefault(e => e.Id == id);
        }

        public void Add(T entity)
        {
            entity.Id = _nextId++;
            _entities.Add(entity);
        }

        public void Update(T entity)
        {
            var existingEntity = _entities.FirstOrDefault(e => e.Id == entity.Id);
            var index = _entities.IndexOf(existingEntity);
            _entities[index] = entity;
        }

        public void Delete(int id)
        {
            var entity = _entities.FirstOrDefault(e => e.Id == id);
            _entities.Remove(entity);
        }

        public bool Exists(int id)
        {
            return _entities.Any(e => e.Id == id);
        }
    }

    class Program
    {
        private static readonly IRepository<Product> _productRepository = new InMemoryRepository<Product>();

        static void Main(string[] args)
        {
            Console.WriteLine("Product Repository Demo");
            Console.WriteLine("----------------------");

            bool running = true;
            while (running)
            {
                DisplayMenu();
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            ListAllProducts();
                            break;
                        case "2":
                            AddProduct();
                            break;
                        case "3":
                            UpdateProduct();
                            break;
                        case "4":
                            DeleteProduct();
                            break;
                        case "5":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("\n1. List all products");
            Console.WriteLine("2. Add new product");
            Console.WriteLine("3. Update product");
            Console.WriteLine("4. Delete product");
            Console.WriteLine("5. Exit");
            Console.Write("\nEnter your choice: ");
        }

        static void ListAllProducts()
        {
            var products = _productRepository.GetAll();
            if (!products.Any())
            {
                Console.WriteLine("No products found.");
                return;
            }

            Console.WriteLine("\nProduct List:");
            Console.WriteLine("-------------");
            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
        }

        static void AddProduct()
        {
            Console.WriteLine("\nAdd New Product");
            Console.WriteLine("---------------");

            var product = new Product();

            Console.Write("Enter product name: ");
            product.Name = Console.ReadLine();

            Console.Write("Enter product price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                throw new ArgumentException("Invalid price format.");
            product.Price = price;

            Console.Write("Enter product category: ");
            product.Category = Console.ReadLine();

            _productRepository.Add(product);
            Console.WriteLine("Product added successfully!");
        }

        static void UpdateProduct()
        {
            Console.WriteLine("\nUpdate Product");
            Console.WriteLine("-------------");

            Console.Write("Enter product ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new ArgumentException("Invalid ID format.");

            var product = _productRepository.GetById(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            Console.WriteLine($"\nCurrent product details: {product}");
            Console.WriteLine("\nEnter new details (press Enter to keep current value):");

            Console.Write("Enter new name: ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                product.Name = name;

            Console.Write("Enter new price: ");
            var priceStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(priceStr) && decimal.TryParse(priceStr, out decimal price))
                product.Price = price;

            Console.Write("Enter new category: ");
            var category = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(category))
                product.Category = category;

            _productRepository.Update(product);
            Console.WriteLine("Product updated successfully!");
        }

        static void DeleteProduct()
        {
            Console.WriteLine("\nDelete Product");
            Console.WriteLine("-------------");

            Console.Write("Enter product ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new ArgumentException("Invalid ID format.");

            _productRepository.Delete(id);
            Console.WriteLine("Product deleted successfully!");
        }
    }
}