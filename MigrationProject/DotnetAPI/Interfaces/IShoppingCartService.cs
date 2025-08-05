using System.Threading.Tasks;
using DotnetAPI.Models;
using System.Collections.Generic;
using DotnetAPI.DTOs.ShoppingCart;

namespace DotnetAPI.Interfaces
{
    public interface IShoppingCartService
    {
        Task<object> AddToCartAsync(AddToCartDTO dto);
        Task<object> UpdateCartAsync(UpdateCartDTO dto);
        Task<object> CheckoutAsync(CheckoutDTO dto);
        
    }
}
