using DotnetAPI.Models;
using DotnetAPI.DTOs.ContactUs;

namespace DotnetAPI.Interfaces
{
    public interface IContactUsService
    {
        Task<ContactUs> AddContactUsAsync(AddContactUsDTO contact);
        Task<ContactUs> UpdateContactUsAsync(int id, UpdateContactUsDTO contact);
        Task<ContactUs> DeleteContactUsAsync(int id);
        Task<ContactUs> GetContactUsByIdAsync(int id);
        Task<IEnumerable<ContactUs>> GetAllContactUsAsync(int pageNumber, int pageSize);
    }
}
