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
            user.UserName = addRequestDto.UserName;
            user.PasswordHash = addRequestDto.PasswordHash;
            user.DateOfBirth = addRequestDto.DateOfBirth;
            user.PhoneNumber = addRequestDto.PhoneNumber;

            return user;
        }

        public User? MapUserUpdateRequest(UserUpdateRequestDTO updateRequestDto)
        {
            User user = new();
            user.Id = updateRequestDto.UserId;
            user.FirstName = updateRequestDto.FirstName;
            user.LastName = updateRequestDto.LastName;
            user.Email = updateRequestDto.Email;
            user.PhoneNumber = updateRequestDto.PhoneNumber;
            user.Address = updateRequestDto.Address;
            user.PANNumber = updateRequestDto.PANNumber;

            return user;
        }
    }
            
}