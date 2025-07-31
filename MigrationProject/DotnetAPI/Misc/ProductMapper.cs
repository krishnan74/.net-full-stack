using DotnetAPI.Models;
using DotnetAPI.DTOs.Product;

namespace DotnetAPI.Misc.Mappers
{
    public class ProductMapper
    {
        public Product? MapAddProduct(AddProductDTO addRequestDto)
        {
            if (addRequestDto == null)
                return null;
            Product product = new();
            product.ProductName = addRequestDto.ProductName;
            product.Image = addRequestDto.Image;
            product.Price = addRequestDto.Price;
            product.UserId = addRequestDto.UserId;
            product.CategoryId = addRequestDto.CategoryId;
            product.ColorId = addRequestDto.ColorId;
            product.ModelId = addRequestDto.ModelId;
            product.StorageId = addRequestDto.StorageId;
            product.SellStartDate = addRequestDto.SellStartDate;
            product.SellEndDate = addRequestDto.SellEndDate;
            product.IsNew = addRequestDto.IsNew;
            return product;
        }

        public Product? MapUpdateProduct(Product existingProduct, UpdateProductDTO updateRequestDto)
        {
            if (existingProduct == null || updateRequestDto == null)
                return null;
            existingProduct.ProductName = updateRequestDto.ProductName;
            existingProduct.Image = updateRequestDto.Image;
            existingProduct.Price = updateRequestDto.Price;
            existingProduct.UserId = updateRequestDto.UserId;
            existingProduct.CategoryId = updateRequestDto.CategoryId;
            existingProduct.ColorId = updateRequestDto.ColorId;
            existingProduct.ModelId = updateRequestDto.ModelId;
            existingProduct.StorageId = updateRequestDto.StorageId;
            existingProduct.SellStartDate = updateRequestDto.SellStartDate;
            existingProduct.SellEndDate = updateRequestDto.SellEndDate;
            existingProduct.IsNew = updateRequestDto.IsNew;
            return existingProduct;
        }
    }
}
