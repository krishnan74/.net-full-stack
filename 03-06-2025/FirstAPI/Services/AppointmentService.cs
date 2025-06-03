using FirstAPI.Repositories;
using FirstAPI.Models;
using FirstAPI.Interfaces;
using FirstAPI.Misc;
using FirstAPI.Models.DTOs.Appointment;

namespace FirstAPI.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<string, Appointment> _appointmentRepository;
        AppointmentMapper appointmentMapper;

        public AppointmentService(IRepository<string, Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
            appointmentMapper = new AppointmentMapper();
        }

        public async Task<Appointment> AddAppointment(AppointmentAddRequestDTO appointment)
        {
            try
            {
                var newAppointment = appointmentMapper.MapAppointmentAddRequestToAppointment(appointment);
                Console.WriteLine(newAppointment.ToString());
                if (newAppointment == null)
                    throw new Exception("Could not map appointment");

                Console.WriteLine("Mapped appointment successfully");

                newAppointment = await _appointmentRepository.Add(newAppointment);

                Console.WriteLine(newAppointment.ToString());

                if (newAppointment == null)
                    throw new Exception("Could not add appointment");

                Console.WriteLine("Added appointment successfully");
                

                return newAppointment;
            }

            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the appointment", ex);
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<Appointment> CancelAppointment(string appointmentNumber, int doctorId)
        {
            try
            {
                var appointment = await _appointmentRepository.Get(appointmentNumber);
                if (appointment == null)
                    throw new Exception("Appointment not found");

                if (appointment.DoctorId != doctorId)
                    throw new Exception("You are not authorized to cancel this appointment");

                appointment.Status = "Cancelled";
                appointment = await _appointmentRepository.Update(appointmentNumber, appointment);
                if (appointment == null)
                    throw new Exception("Could not cancel appointment");

                return appointment;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while cancelling the appointment", ex);
            }
        }
        

    }
}