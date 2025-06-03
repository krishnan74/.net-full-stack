using System.Collections.Generic;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using FirstAPI.Models;

namespace FirstAPI.Interfaces
{
    public interface IDoctorService
    {
        public Task<Doctor> AddDoctor(DoctorAddRequestDTO doctor);
        public Task<Doctor> GetDoctorByName(string name);
        public Task<ICollection<DoctorsBySpecialityResponseDTO>> GetDoctorsBySpeciality(string speciality);
        public Task<Doctor> GetDoctorByEmail(string email);
    }
}