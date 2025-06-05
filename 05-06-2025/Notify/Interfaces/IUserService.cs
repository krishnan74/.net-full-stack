using Notify.Models;

namespace Notify.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User> CreateUserAsync(User user);
    // Task<User> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
} 