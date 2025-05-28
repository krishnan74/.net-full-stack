using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;

namespace FirstAPI.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, Speciality> _specialityRepository;
        private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;

        public DoctorService(
            IRepository<int, Doctor> doctorRepository,
            IRepository<int, Speciality> specialityRepository,
            IRepository<int, DoctorSpeciality> doctorSpecialityRepository
        )
        {
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
            _doctorSpecialityRepository = doctorSpecialityRepository;
        }

       public async Task<Doctor> AddDoctor(DoctorAddRequestDTO doctor)
        {
            try
            {
                Doctor newDoc = new Doctor
                {
                    Name = doctor.Name,
                    YearsOfExperience = doctor.YearsOfExperience,
                    Status = "Active"
                };
                Doctor addedDoc = await _doctorRepository.Add(newDoc);
                if (doctor.Specialities != null && doctor.Specialities.Count() > 0)
                {
                    IEnumerable<Speciality> specialities = await _specialityRepository.GetAll();
                    foreach (var speciality in doctor.Specialities)
                    {
                        var v_spec = specialities.FirstOrDefault(s => s.Name == speciality.Name);
                        if (v_spec == null)
                        {
                            v_spec = await _specialityRepository.Add(new Speciality { Name = speciality.Name, Status = "Active" });
                        }
                        await _doctorSpecialityRepository.Add(new DoctorSpeciality { DoctorId = doc.Id, SpecialityId = v_spec.Id });
                    }
                }
                return addedDoc;
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<Doctor> GetDoctorByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Doctor name cannot be null or empty", nameof(name));

                var doctors = await _doctorRepository.GetAll();
                var doctor = doctors.FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                return doctor;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<ICollection<Doctor>> GetDoctorsBySpeciality(string speciality)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(speciality))
                    throw new ArgumentException("Speciality cannot be null or empty", nameof(speciality));

                var specialities = await _specialityRepository.GetAll();
                var specialityEntity = specialities.FirstOrDefault(s => s.Name.Equals(speciality, StringComparison.OrdinalIgnoreCase));
                if (specialityEntity == null)
                    return Task.FromResult<ICollection<Doctor>>(new List<Doctor>());

                var doctorSpecialities = await _doctorSpecialityRepository.GetAll()
                    .Where(ds => ds.SpecialityId == specialityEntity.Id);

                var doctors = await _doctorRepository.GetAll()
                    .Where(d => doctorSpecialities.Any(ds => ds.DoctorId == d.Id));
                    

                return doctors;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Doctor>();
            }
        }

    }
}