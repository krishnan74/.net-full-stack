using DotnetAPI.Models;
using DotnetAPI.DTOs.User;

namespace DotnetAPI.Misc.Mappers
{
    public class UserMapper
    {
        public User? MapAddUser(AddUserDTO addRequestDto)
        {
            if (addRequestDto == null)
                return null;
            User user = new();
            user.Username = addRequestDto.Username;
            user.Password = addRequestDto.Password;
            return user;
        }

        public User? MapUpdateUser(User existingUser, UpdateUserDTO updateRequestDto)
        {
            if (existingUser == null || updateRequestDto == null)
                return null;
            existingUser.Username = updateRequestDto.Username;
            existingUser.Password = updateRequestDto.Password;
            return existingUser;
        }
    }
}
