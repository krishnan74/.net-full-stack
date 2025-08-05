using DotnetAPI.Models;
using DotnetAPI.DTOs.User;

namespace DotnetAPI.Interfaces
{
    public interface IUserService
    {
        Task<User> AddUserAsync(AddUserDTO user);
        Task<User> UpdateUserAsync(int id, UpdateUserDTO user);
        Task<User> DeleteUserAsync(int id);
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, int pageSize);
    }
}
