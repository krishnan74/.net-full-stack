using FirstAPI.Interfaces;
using AutoMapper;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.Patient;
using FirstAPI.Misc;

namespace FirstAPI.Services
{
    public class PatientService : IPatientService
    {

        PatientMapper patientMapper;
        private readonly IRepository<int, Patient> _patientRepository;
        private readonly IRepository<string, User> _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;

        public PatientService(IRepository<int, Patient> patientRepository,
                            IRepository<string, User> userRepository,
                            IEncryptionService encryptionService,
                            IMapper mapper
                            )
        {
            patientMapper = new PatientMapper();
            _patientRepository = patientRepository;
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _mapper = mapper;
        }

        public async Task<Patient> AddPatient(PatientAddRequestDTO patient)
        {
            try
            {

                var user = _mapper.Map<PatientAddRequestDTO, User>(patient);
                var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                {
                    Data = patient.Password,
                });

                user.Password = encryptedData.EncryptedData;
                user.HashKey = encryptedData.HashKey;
                user.Role = "Patient";
                user = await _userRepository.Add(user);

                var newPatient = patientMapper.MapPatientAddRequestPatient(patient);
                newPatient = await _patientRepository.Add(newPatient);
                if (newPatient == null)
                    throw new Exception("Could not add patient");
                

                return newPatient;
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        public async Task<Patient> GetPatientByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Patient name cannot be null or empty", nameof(name));

                var patients = await _patientRepository.GetAll();
                var patient = patients.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                return patient;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<ICollection<PatientsByDiagnosisResponseDTO>> GetPatientsByDiagnosis(string diagnosis)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(diagnosis))
                {
                    throw new ArgumentException("Diagnosis cannot be null or empty", nameof(diagnosis));
                }

                var result = await _patientRepository.GetAll();

                result = result.Where(p => p.Diagnosis.Equals(diagnosis, StringComparison.OrdinalIgnoreCase)).ToList();
                
                var dto_result = _mapper.Map<IEnumerable<Patient>, ICollection<PatientsByDiagnosisResponseDTO>>(result);
                if (result == null || !result.Any())
                    return new List<PatientsByDiagnosisResponseDTO>();

                return dto_result;

            }
            
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<PatientsByDiagnosisResponseDTO>();
            }
        
        }

    }
}