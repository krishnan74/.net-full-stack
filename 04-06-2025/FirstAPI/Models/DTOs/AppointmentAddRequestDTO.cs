namespace FirstAPI.Models.DTOs.Appointment
{
    public class AppointmentAddRequestDTO
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime AppointmentDateTime { get; set; }

    }
}