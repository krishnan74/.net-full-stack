using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.Contexts;
using DotnetAPI.DTOs.User;
using DotnetAPI.Misc.Mappers;

namespace DotnetAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<int, Model> _modelRepository;
        public UserMapper userMapper;
        public UserService(IRepository<int, User> userRepository, IRepository<int, Model> modelRepository)
        {
            _userRepository = userRepository;
            _modelRepository = modelRepository;
            userMapper = new UserMapper();
        }

        public async Task<User> AddUserAsync(AddUserDTO userDTO)
        {
            if (userDTO == null)
                throw new ArgumentNullException(nameof(userDTO));
            if (string.IsNullOrWhiteSpace(userDTO.Username))
                throw new ArgumentException("User name cannot be null or empty.");
            // Add more validations as needed (e.g., price, category, etc.)

            var newUser = userMapper.MapAddUser(userDTO);
            if (newUser == null)
                throw new Exception("Failed to map AddUserDTO to User.");
            var addedUser = await _userRepository.Add(newUser);
            if (addedUser == null)
                throw new Exception("Failed to add new user.");
            return addedUser;
        }

        public async Task<User> UpdateUserAsync(int id, UpdateUserDTO userDTO)
        {
            if (userDTO == null)
                throw new ArgumentNullException(nameof(userDTO));
            var user = await _userRepository.Get(id);
            if (user == null)
                throw new KeyNotFoundException($"No user with the given ID: {id}");
            var updatedUser = userMapper.MapUpdateUser(user, userDTO);
            if (updatedUser == null)
                throw new Exception("Failed to map UpdateUserDTO to User.");
            var result = await _userRepository.Update(id, updatedUser);
            if (result == null)
                throw new Exception("Failed to update user.");
            return result;
        }

        public async Task<User> DeleteUserAsync(int id)
        {
            var deletedUser = await _userRepository.Delete(id);
            return deletedUser;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.Get(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            return await _userRepository.GetAll(pageNumber, pageSize);
        }
    }
}
