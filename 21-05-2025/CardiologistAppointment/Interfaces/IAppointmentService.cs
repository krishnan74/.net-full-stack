using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardiologistAppointment.Models;

namespace CardiologistAppointment.Interfaces
{
    public interface IAppointmentService
    {
        int AddAppointment(Appointment appointment);
        List<Appointment>? SearchAppointment( AppointmentSearchModel appointmentSearchModel );
    }
}
