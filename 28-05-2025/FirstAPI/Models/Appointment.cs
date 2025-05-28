using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAPI.Models
{
    public class Appointment
    {

        public string AppointmentNumber { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime AppointmentDateTime { get; set; }
        public string Status { get; set; } = string.Empty;
        
        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
    }
}