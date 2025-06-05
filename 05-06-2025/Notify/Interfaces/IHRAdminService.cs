using System.Collections.Generic;
using Notify.Models.DTOs;
using Notify.Models;

namespace Notify.Interfaces
{
    public interface IHRAdminService
    {
        Task<HRAdmin?> CreateHRAdminAsync(HRAdminAddRequestDTO request);
        Task<HRAdmin?> GetHRAdminByIdAsync(int id);
        public Task<HRAdmin?> GetHRAdminByName(string name);
        public Task<ICollection<HRAdminsByDepartmentResponseDTO>> GetHRAdminsByDepartment(string department);
    }
}