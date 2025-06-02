namespace FirstAPI.Models.DTOs.Patient
{
    public class PatientsByDiagnosisResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Age { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
    }
}