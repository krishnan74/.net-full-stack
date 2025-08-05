using DotnetAPI.Models;
using DotnetAPI.DTOs.ContactUs;

namespace DotnetAPI.Misc.Mappers
{
    public class ContactUsMapper
    {
        public ContactUs? MapAddContactUs(AddContactUsDTO addRequestDto)
        {
            if (addRequestDto == null)
                return null;
            ContactUs contact = new();
            contact.Name = addRequestDto.Name;
            contact.Email = addRequestDto.Email;
            contact.Phone = addRequestDto.Phone;
            contact.Content = addRequestDto.Content;
            return contact;
        }

        public ContactUs? MapUpdateContactUs(ContactUs existingContact, UpdateContactUsDTO updateRequestDto)
        {
            if (existingContact == null || updateRequestDto == null)
                return null;
            existingContact.Name = updateRequestDto.Name;
            existingContact.Email = updateRequestDto.Email;
            existingContact.Phone = updateRequestDto.Phone;
            existingContact.Content = updateRequestDto.Content;
            return existingContact;
        }
    }
}
