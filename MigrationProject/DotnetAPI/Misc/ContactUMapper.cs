using DotnetAPI.Models;
using DotnetAPI.DTOs.ContactU;

namespace DotnetAPI.Misc.Mappers
{
    public class ContactUMapper
    {
        public ContactU? MapAddContactU(AddContactUDTO addRequestDto)
        {
            if (addRequestDto == null)
                return null;
            ContactU contact = new();
            contact.name = addRequestDto.name;
            contact.email = addRequestDto.email;
            contact.phone = addRequestDto.phone;
            contact.content = addRequestDto.content;
            return contact;
        }

        public ContactU? MapUpdateContactU(ContactU existingContact, UpdateContactUDTO updateRequestDto)
        {
            if (existingContact == null || updateRequestDto == null)
                return null;
            existingContact.name = updateRequestDto.name;
            existingContact.email = updateRequestDto.email;
            existingContact.phone = updateRequestDto.phone;
            existingContact.content = updateRequestDto.content;
            return existingContact;
        }
    }
}
