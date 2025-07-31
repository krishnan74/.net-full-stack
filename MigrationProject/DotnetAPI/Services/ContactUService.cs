using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.Contexts;
using DotnetAPI.DTOs.ContactU;
using DotnetAPI.Misc.Mappers;

namespace DotnetAPI.Services
{
    public class ContactUService : IContactUService
    {
        private readonly IRepository<int, ContactU> _contactURepository;
        public ContactUMapper contactUMapper;
        private readonly DotnetAPIContext _context;
        public ContactUService(IRepository<int, ContactU> contactURepository, DotnetAPIContext context)
        {
            _contactURepository = contactURepository;
            contactUMapper = new ContactUMapper();
            _context = context;
        }

        public async Task<ContactU> AddContactUAsync(AddContactUDTO contactDTO)
        {
            if (contactDTO == null)
                throw new ArgumentNullException(nameof(contactDTO));
            if (string.IsNullOrWhiteSpace(contactDTO.name))
                throw new ArgumentException("Name cannot be null or empty.");
            var newContact = contactUMapper.MapAddContactU(contactDTO);
            if (newContact == null)
                throw new Exception("Failed to map AddContactUDTO to ContactU.");
            var addedContact = await _contactURepository.Add(newContact);
            if (addedContact == null)
                throw new Exception("Failed to add new contact.");
            return addedContact;
        }

        public async Task<ContactU> UpdateContactUAsync(int id, UpdateContactUDTO contactDTO)
        {
            if (contactDTO == null)
                throw new ArgumentNullException(nameof(contactDTO));
            var contact = await _contactURepository.Get(id);
            if (contact == null)
                throw new KeyNotFoundException($"No contact with the given ID: {id}");
            var updatedContact = contactUMapper.MapUpdateContactU(contact, contactDTO);
            if (updatedContact == null)
                throw new Exception("Failed to map UpdateContactUDTO to ContactU.");
            var result = await _contactURepository.Update(id, updatedContact);
            if (result == null)
                throw new Exception("Failed to update contact.");
            return result;
        }

        public async Task<ContactU> DeleteContactUAsync(int id)
        {
            var deletedContact = await _contactURepository.Delete(id);
            return deletedContact;
        }

        public async Task<ContactU> GetContactUByIdAsync(int id)
        {
            return await _contactURepository.Get(id);
        }

        public async Task<IEnumerable<ContactU>> GetAllContactUsAsync(int pageNumber, int pageSize)
        {
            return await _contactURepository.GetAll(pageNumber, pageSize);
        }
    }
}
