using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;

namespace FirstAPI.Misc
{
    public class DoctorMapper
    {
        public Doctor? MapDoctorAddRequestDoctor(DoctorAddRequestDTO addRequestDto)
        {
            Doctor doctor = new();
            doctor.Name = addRequestDto.Name;
            doctor.YearsOfExperience = addRequestDto.YearsOfExperience;
            doctor.Email = addRequestDto.Email;

            return doctor;
        }
    }
}