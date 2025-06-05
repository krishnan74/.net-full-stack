using Microsoft.EntityFrameworkCore;
using Notify.Contexts;
using Notify.Interfaces;
using Notify.Models;

namespace Notify.Services;

public class UserService : IUserService
{
    private readonly NotifyContext _context;
    private readonly IEncryptionService _encryptionService;

    public UserService(NotifyContext context, IEncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.HRAdmin)
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        // Hash the password before saving
        // user.PasswordHash = await _encryptionService.HashPasswordAsync(user.PasswordHash);
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    // public async Task<User> UpdateUserAsync(User user)
    // {
    //     var existingUser = await _context.Users.FindAsync(user.Id);
    //     if (existingUser == null)
    //         throw new KeyNotFoundException($"User with ID {user.Id} not found");

    //     // Update only the allowed fields
    //     existingUser.Username = user.Username;
    //     existingUser.Email = user.Email;
    //     existingUser.Role = user.Role;
        
    //     if (!string.IsNullOrEmpty(user.PasswordHash))
    //     {
    //         existingUser.PasswordHash = await _encryptionService.HashPasswordAsync(user.PasswordHash);
    //     }

    //     await _context.SaveChangesAsync();
    //     return existingUser;
    // }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
    
} 