using Microsoft.EntityFrameworkCore;
using BankApplication.Models;
using BankApplication.Models.DTO;
using BankApplication.Interfaces;
using BankApplication.Misc;

namespace BankApplication.Services
{
    public class UserService : IUserService
    {

        UserMapper userMapper;
        private readonly IRepository<int, User> _userRepository;

        public UserService(IRepository<int, User> userRepository)
        {
            _userRepository = userRepository;
            userMapper = new UserMapper();
        }

        public async Task<User> RegisterUserAsync(UserAddRequestDTO userAddRequest)
        {
            try
            {

                var users = await _userRepository.GetAll();
                var existingUser = users.FirstOrDefault(u => u.Username == userAddRequest.Username);
                if (existingUser != null)
                {
                    throw new Exception("User already exists.");
                }

                var user = userMapper.MapUserAddRequest(userAddRequest);


                var createdUser = await _userRepository.Add(user);
                return createdUser ?? throw new Exception("User registration failed");
            }

            catch (Exception ex)
            {
                throw new Exception($"Error registering user: {ex.Message}");
            }

        }

        public async Task<User> UpdateUserDetailsAsync(UserUpdateRequestDTO userUpdateRequest)
        {
            try
            {
                var existingUser = await _userRepository.Get(userUpdateRequest.UserId);
                if (existingUser == null)
                {
                    throw new Exception("User not found.");
                }

                var user = userMapper.MapUserUpdateRequest(userUpdateRequest, existingUser);
                if (user == null)
                {
                    throw new Exception("Invalid user update request.");
                }

                var updatedUser = await _userRepository.Update(user.Id, user);
                return updatedUser ?? throw new Exception("User update failed");
            }

            catch (Exception ex)
            {
                throw new Exception($"Error updating user: {ex.Message}");
            }
        }

        public async Task<bool> LoginUserAsync(UserLoginRequestDTO userLoginRequest)
        {
            try
            {
                var users = await _userRepository.GetAll();
                var user = users.FirstOrDefault(u => u.Username == userLoginRequest.Username && u.PasswordHash == userLoginRequest.Password);
                if (user == null)
                {
                    throw new Exception("Invalid username or password.");
                }
                user.SessionActive = true; 
                await _userRepository.Update(user.Id, user); 
                return true; 
            }
            catch (Exception ex)
            {
                throw new Exception($"Error logging in user: {ex.Message}");
            }
        }
        
        public async Task<bool> LogoutUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.Get(userId);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }
                user.SessionActive = false;
                await _userRepository.Update(user.Id, user);
                return true; 
            }
            catch (Exception ex)
            {
                throw new Exception($"Error logging out user: {ex.Message}");
            }
        }

    }
}