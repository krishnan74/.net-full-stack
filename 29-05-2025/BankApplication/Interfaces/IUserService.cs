using BankApplication.Models;
using BankApplication.Models.DTO;

namespace BankApplication.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(UserAddRequestDTO userAddRequest);
        Task<bool> LoginUserAsync(string username, string password);
        Task<bool> LogoutUserAsync(int userId);
        Task<User> UpdateUserDetailsAsync(UserUpdateRequestDTO userUpdateRequest);
    }
}