using BankApplication.Models;
using BankApplication.Models.DTO;

namespace BankApplication.Misc
{
    public class BankMapper
    {
        public Bank? MapBankAddRequest(BankAddRequestDTO addRequestDto)
        {
            Bank bank = new();
            bank.Name = addRequestDto.Name;
            bank.ContactNumber = addRequestDto.ContactNumber;
            bank.Email = addRequestDto.Email;
            return bank;
        }
    }
}