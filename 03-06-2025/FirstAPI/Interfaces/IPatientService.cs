using System.Collections.Generic;
using FirstAPI.Models.DTOs.Patient;
using FirstAPI.Models;

namespace FirstAPI.Interfaces
{
    public interface IPatientService
    {
        public Task<Patient> AddPatient(PatientAddRequestDTO patient);
        public Task<Patient> GetPatientByName(string name);
        public Task<ICollection<PatientsByDiagnosisResponseDTO>> GetPatientsByDiagnosis(string diagnosis);

    }
}