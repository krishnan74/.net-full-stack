using DotnetAPI.Models;
using DotnetAPI.DTOs.ContactU;

namespace DotnetAPI.Interfaces
{
    public interface IContactUService
    {
        Task<ContactU> AddContactUAsync(AddContactUDTO contact);
        Task<ContactU> UpdateContactUAsync(int id, UpdateContactUDTO contact);
        Task<ContactU> DeleteContactUAsync(int id);
        Task<ContactU> GetContactUByIdAsync(int id);
        Task<IEnumerable<ContactU>> GetAllContactUsAsync(int pageNumber, int pageSize);
    }
}
