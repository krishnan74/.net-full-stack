using Notify.Interfaces;
using AutoMapper;
using Notify.Models;
using Notify.Models.DTOs;
using Notify.Misc;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Notify.Services
{
    public class HRAdminService : IHRAdminService
    {
        private readonly IRepository<int, HRAdmin> _hrAdminRepository;
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger<HRAdminService> _logger;
        private readonly IMapper _mapper;

        public HRAdminService(
            IRepository<int, HRAdmin> hrAdminRepository,
            IUserService userService,
            IEncryptionService encryptionService,
            ILogger<HRAdminService> logger,
            IMapper mapper)
        {
            _hrAdminRepository = hrAdminRepository;
            _userService = userService;
            _encryptionService = encryptionService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<HRAdmin> CreateHRAdminAsync(HRAdminAddRequestDTO hrAdminDto)
        {
            try
            {
                var user = _mapper.Map<HRAdminAddRequestDTO, User>(hrAdminDto);

                var createdUser = await _userService.CreateUserAsync(user);
                var hrAdmin = hrAdminDto.ToHRAdmin(createdUser.Username);

                return await _hrAdminRepository.Add(hrAdmin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating HR admin");
                throw;
            }
        }

        public async Task<HRAdmin?> GetHRAdminByIdAsync(int id)
        {
            try
            {
                return await _hrAdminRepository.Get(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting HR admin by id {Id}", id);
                throw;
            }
        }

        public async Task<HRAdmin?> GetHRAdminByNameAsync(string name)
        {
            try
            {
                var hrAdmins = await _hrAdminRepository.GetAll();
                return hrAdmins.FirstOrDefault(h => h.Name == name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting HR admin by name {Name}", name);
                throw;
            }
        }

        public async Task<IEnumerable<HRAdmin>> GetAllHRAdminsAsync()
        {
            try
            {
                return await _hrAdminRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all HR admins");
                throw;
            }
        }

        public async Task<HRAdmin> UpdateHRAdminAsync(int id, HRAdminUpdateRequestDTO hrAdminDto)
        {
            try
            {
                var hrAdmin = await _hrAdminRepository.Get(id);
                if (hrAdmin == null)
                {
                    throw new KeyNotFoundException($"HR Admin with id {id} not found");
                }

                hrAdmin.FirstName = hrAdminDto.FirstName;
                hrAdmin.LastName = hrAdminDto.LastName;
                hrAdmin.Department = hrAdminDto.Department;
                hrAdmin.Position = hrAdminDto.Position;
                hrAdmin.PhoneNumber = hrAdminDto.PhoneNumber;

                return await _hrAdminRepository.Update(id, hrAdmin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating HR admin {Id}", id);
                throw;
            }
        }

        public async Task<HRAdmin> DeleteHRAdminAsync(int id)
        {
            try
            {
                return await _hrAdminRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting HR admin {Id}", id);
                throw;
            }
        }


        public async Task<HRAdmin> GetHRAdminByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("HRAdmin name cannot be null or empty", nameof(name));

                var hrAdmins = await _hrAdminRepository.GetAll();
                var hrAdmin = hrAdmins.FirstOrDefault(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                return hrAdmin;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<ICollection<HRAdminsByDepartmentResponseDTO>> GetHRAdminsByDepartment(string department)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(department))
                {
                    throw new ArgumentException("Department cannot be null or empty", nameof(department));
                }

                var result = await _hrAdminRepository.GetAll();

                result = result.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase)).ToList();
                
                var dto_result = _mapper.Map<IEnumerable<HRAdmin>, ICollection<HRAdminsByDepartmentResponseDTO>>(result);
                
                if (result == null || !result.Any())
                    return new List<HRAdminsByDepartmentResponseDTO>();

                return dto_result;

            }
            
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<HRAdminsByDepartmentResponseDTO>();
            }
        
        }
    }
}
