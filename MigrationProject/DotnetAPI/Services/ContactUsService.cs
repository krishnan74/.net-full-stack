using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.Contexts;
using DotnetAPI.DTOs.ContactUs;
using DotnetAPI.Misc.Mappers;

namespace DotnetAPI.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IRepository<int, ContactUs> _contactUsRepository;
        public ContactUsMapper contactUsMapper;
        private readonly DotnetAPIContext _context;
        public ContactUsService(IRepository<int, ContactUs> contactUsRepository, DotnetAPIContext context)
        {
            _contactUsRepository = contactUsRepository;
            contactUsMapper = new ContactUsMapper();
            _context = context;
        }

        public async Task<ContactUs> AddContactUsAsync(AddContactUsDTO contactDTO)
        {
            if (contactDTO == null)
                throw new ArgumentNullException(nameof(contactDTO));
            if (string.IsNullOrWhiteSpace(contactDTO.Name))
                throw new ArgumentException("Name cannot be null or empty.");
            var newContact = contactUsMapper.MapAddContactUs(contactDTO);
            if (newContact == null)
                throw new Exception("Failed to map AddContactUsDTO to ContactUs.");
            var addedContact = await _contactUsRepository.Add(newContact);
            if (addedContact == null)
                throw new Exception("Failed to add new contact.");
            return addedContact;
        }

        public async Task<ContactUs> UpdateContactUsAsync(int id, UpdateContactUsDTO contactDTO)
        {
            if (contactDTO == null)
                throw new ArgumentNullException(nameof(contactDTO));
            var contact = await _contactUsRepository.Get(id);
            if (contact == null)
                throw new KeyNotFoundException($"No contact with the given ID: {id}");
            var updatedContact = contactUsMapper.MapUpdateContactUs(contact, contactDTO);
            if (updatedContact == null)
                throw new Exception("Failed to map UpdateContactUsDTO to ContactUs.");
            var result = await _contactUsRepository.Update(id, updatedContact);
            if (result == null)
                throw new Exception("Failed to update contact.");
            return result;
        }

        public async Task<ContactUs> DeleteContactUsAsync(int id)
        {
            var deletedContact = await _contactUsRepository.Delete(id);
            return deletedContact;
        }

        public async Task<ContactUs> GetContactUsByIdAsync(int id)
        {
            return await _contactUsRepository.Get(id);
        }

        public async Task<IEnumerable<ContactUs>> GetAllContactUsAsync(int pageNumber, int pageSize)
        {
            return await _contactUsRepository.GetAll(pageNumber, pageSize);
        }
    }
}
