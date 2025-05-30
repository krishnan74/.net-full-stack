using BankApplication.Models;
using BankApplication.Models.DTO;

namespace BankApplication.Misc
{
    public class UserMapper
    {
        public User? MapUserAddRequest(UserAddRequestDTO addRequestDto)
        {
            User user = new();
            user.FirstName = addRequestDto.FirstName;
            user.LastName = addRequestDto.LastName;
            user.Username = addRequestDto.Username;
            user.PasswordHash = addRequestDto.PasswordHash;
            user.DateOfBirth = addRequestDto.DateOfBirth;
            user.PhoneNumber = addRequestDto.PhoneNumber;

            return user;
        }

        public User? MapUserUpdateRequest(UserUpdateRequestDTO updateRequestDto, User existingUser)
        {
            
            existingUser.FirstName = updateRequestDto.FirstName;
            existingUser.LastName = updateRequestDto.LastName;
            existingUser.Email = updateRequestDto.Email;
            existingUser.PhoneNumber = updateRequestDto.PhoneNumber;
            existingUser.Address = updateRequestDto.Address;
            existingUser.PANNumber = updateRequestDto.PANNumber;

            return existingUser;
        }
    }
            
}