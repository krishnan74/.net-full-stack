using FirstAPI.Models;
using FirstAPI.Models.DTOs.Patient;

namespace FirstAPI.Misc
{
    public class PatientMapper
    {
        public Patient? MapPatientAddRequestPatient(PatientAddRequestDTO addRequestDto)
        {
            Patient patient = new();
            patient.Name = addRequestDto.Name;
            patient.Diagnosis = addRequestDto.Diagnosis;
            patient.Email = addRequestDto.Email;

            return patient;
        }
    }
}