using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.Appointment;

namespace FirstAPI.Interfaces
{
    public interface IAppointmentService
    {
        public Task<Appointment> AddAppointment(AppointmentAddRequestDTO appointment);
        public Task<Appointment> CancelAppointment(string appointmentNumber, int doctorId);
        // public Task<List<Appointment>>? SearchAppointment( AppointmentSearchModel appointmentSearchModel );
    }
}
