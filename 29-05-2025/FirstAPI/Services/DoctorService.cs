using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using FirstAPI.Misc;

namespace FirstAPI.Services
{
    public class DoctorService : IDoctorService
    {
        DoctorMapper doctorMapper;
        SpecialityMapper specialityMapper;
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, Speciality> _specialityRepository;
        private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;
        private readonly IOtherContextFunctionalities _otherContextFunctionalities;

        public DoctorService(IRepository<int, Doctor> doctorRepository,
                            IRepository<int, Speciality> specialityRepository,
                            IRepository<int, DoctorSpeciality> doctorSpecialityRepository,
                            IOtherContextFunctionalities otherContextFunctionalities)
        {
            doctorMapper = new DoctorMapper();
            specialityMapper = new();
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
            _doctorSpecialityRepository = doctorSpecialityRepository;
            _otherContextFunctionalities = otherContextFunctionalities;

        }

        public async Task<Doctor> AddDoctor(DoctorAddRequestDTO doctor)
        {
            try
            {
                var newDoctor = doctorMapper.MapDoctorAddRequestDoctor(doctor);
                newDoctor = await _doctorRepository.Add(newDoctor);
                if (newDoctor == null)
                    throw new Exception("Could not add doctor");
                if (doctor.Specialities.Count() > 0)
                {
                    int[] specialities = await MapAndAddSpeciality(doctor);
                    for (int i = 0; i < specialities.Length; i++)
                    {
                        var doctorSpeciality = specialityMapper.MapDoctorSpecility(newDoctor.Id, specialities[i]);
                        doctorSpeciality = await _doctorSpecialityRepository.Add(doctorSpeciality);
                    }
                }

                return newDoctor;
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        private async Task<int[]> MapAndAddSpeciality(DoctorAddRequestDTO doctor)
        {
            int[] specialityIds = new int[doctor.Specialities.Count()];
            IEnumerable<Speciality> existingSpecialities = null;
            try
            {
                existingSpecialities = await _specialityRepository.GetAll();
            }
            catch (Exception e)
            {

            }
            int count = 0;
            foreach (var item in doctor.Specialities)
            {
                Speciality speciality = null;
                if (existingSpecialities != null)
                    speciality = existingSpecialities.FirstOrDefault(s => s.Name.ToLower() == item.Name.ToLower());
                if (speciality == null)
                {
                    speciality = specialityMapper.MapSpecialityAddRequestDoctor(item);
                    speciality = await _specialityRepository.Add(speciality);
                }
                specialityIds[count] = speciality.Id;
                count++;
            }
            return specialityIds;
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

        public async Task<ICollection<DoctorsBySpecialityResponseDTO>> GetDoctorsBySpeciality(string speciality)
        {
            var result = await _otherContextFunctionalities.GetDoctorsBySpeciality(speciality);
            return result;
        }

    }
}