using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstAPI.Models;

namespace FirstAPI.Interfaces
{
    public interface IAppointmentService
    {
        public Task<Appointment> AddAppointment(Appointment appointment);
        // public Task<List<Appointment>>? SearchAppointment( AppointmentSearchModel appointmentSearchModel );
    }
}
