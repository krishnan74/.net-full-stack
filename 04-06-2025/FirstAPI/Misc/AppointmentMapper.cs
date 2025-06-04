using FirstAPI.Models;
using FirstAPI.Models.DTOs.Appointment;

namespace FirstAPI.Misc
{
    public class AppointmentMapper
    {
        public Appointment? MapAppointmentAddRequestToAppointment(AppointmentAddRequestDTO addRequestDTO)
        {
            Appointment appointment = new();
            appointment.PatientId = addRequestDTO.PatientId;
            appointment.DoctorId = addRequestDTO.DoctorId;
            appointment.Description = addRequestDTO.Description;
            appointment.AppointmentDateTime = addRequestDTO.AppointmentDateTime;
            appointment.Status = "Scheduled";
            appointment.AppointmentNumber = GenerateAppointmentNumber();

            return appointment;
        }

        public string GenerateAppointmentNumber()
        {
            // Generate a unique appointment number, e.g., using a GUID or a timestamp
            return Guid.NewGuid().ToString();
        }
    }
}