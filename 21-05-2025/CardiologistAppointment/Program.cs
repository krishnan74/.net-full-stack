using CardiologistAppointment.Interfaces;
using CardiologistAppointment.Repositories;
using CardiologistAppointment.Models;
using CardiologistAppointment.Services;
using CardiologistAppointment;

namespace WholeApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {

            IRepository<int, Appointment> appointmentRepository = new AppointmentRepository();
            IAppointmentService appointmentService = new AppointmentService(appointmentRepository);
            ManageAppointment manageAppointment = new ManageAppointment(appointmentService);
            manageAppointment.Start();
        }
    }
}