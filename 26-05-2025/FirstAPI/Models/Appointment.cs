using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAPI.Models
{
    public class Appointment
    {

        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime AppointmentDateTime { get; set; }
        public string Status { get; set; } = string.Empty;
        
        [ForeignKey("DoctorId")]
        public Doctor? Doctor { get; set; }
        [ForeignKey("PatientId")]
        public Patient? Patient { get; set; }
    }
}