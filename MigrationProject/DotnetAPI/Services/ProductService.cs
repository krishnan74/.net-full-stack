using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.Contexts;
using DotnetAPI.DTOs.Product;
using DotnetAPI.Misc.Mappers;

namespace DotnetAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<int, Product> _productRepository;
        private readonly IRepository<int, Model> _modelRepository;
        public ProductMapper productMapper;
        public ProductService(IRepository<int, Product> productRepository, IRepository<int, Model> modelRepository)
        {
            _productRepository = productRepository;
            _modelRepository = modelRepository;
            productMapper = new ProductMapper();
        }

        public async Task<Product> AddProductAsync(AddProductDTO productDTO, string filePath = null)
        {
            if (productDTO == null)
                throw new ArgumentNullException(nameof(productDTO));
            if (string.IsNullOrWhiteSpace(productDTO.ProductName))
                throw new ArgumentException("Product name cannot be null or empty.");
            // Add more validations as needed (e.g., price, category, etc.)

            var model = new Model
            {
                ModelName = productDTO.ModelName
            };
            var newModel = await _modelRepository.Add(model);
            var newProduct = productMapper.MapAddProduct(productDTO, newModel.ModelId, filePath);
            if (newProduct == null)
                throw new Exception("Failed to map AddProductDTO to Product.");
            var addedProduct = await _productRepository.Add(newProduct);
            if (addedProduct == null)
                throw new Exception("Failed to add new product.");
            return addedProduct;
        }

        public async Task<Product> UpdateProductAsync(int id, UpdateProductDTO productDTO, string filePath = null)
        {
            if (productDTO == null)
                throw new ArgumentNullException(nameof(productDTO));
            var product = await _productRepository.Get(id);
            if (product == null)
                throw new KeyNotFoundException($"No product with the given ID: {id}");
            var updatedProduct = productMapper.MapUpdateProduct(product, productDTO, filePath);
            if (updatedProduct == null)
                throw new Exception("Failed to map UpdateProductDTO to Product.");
            var result = await _productRepository.Update(id, updatedProduct);
            if (result == null)
                throw new Exception("Failed to update product.");
            return result;
        }

        public async Task<Product> DeleteProductAsync(int id)
        {
            var deletedProduct = await _productRepository.Delete(id);
            return deletedProduct;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepository.Get(id);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            return await _productRepository.GetAll(pageNumber, pageSize);
        }
    }
}
