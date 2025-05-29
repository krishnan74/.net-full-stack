using FirstAPI.Models.DTOs.DoctorSpecialities;

namespace FirstAPI.Models.DTOs.DoctorSpecialities
{
    public class DoctorAddRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<SpecialityAddRequestDTO>? Specialities { get; set; }
        public float YearsOfExperience { get; set; }
    }
}