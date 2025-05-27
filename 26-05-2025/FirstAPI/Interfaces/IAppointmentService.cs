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
        int AddAppointment(Appointment appointment);
        Appointment GetAppointmentById(int id);
        Appointment UpdateAppointment(Appointment appointment);
        void DeleteAppointment(int id);
        List<Appointment>? SearchAppointment( AppointmentSearchModel appointmentSearchModel );
    }
}
