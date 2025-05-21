using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardiologistAppointment.Interfaces;
using CardiologistAppointment.Models;
using CardiologistAppointment.Repositories;

namespace CardiologistAppointment.Services
{
    public class AppointmentService : IAppointmentService
    {
        IRepository<int, Appointment> _appointmentRepository;
        public AppointmentService(IRepository<int, Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        public int AddAppointment(Appointment appointment)
        {
            try
            {
                var result = _appointmentRepository.Add(appointment);
                if (result != null)
                {
                    return result.Id;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return -1;
        }

        public List<Appointment>? SearchAppointment(AppointmentSearchModel searchModel)
        {
            try
            {
                var appointments = _appointmentRepository.GetAll();
                appointments = SearchByName(appointments, searchModel.PatientName);
                appointments = SeachByAge(appointments, searchModel.AgeRange);
                appointments = SearchByDate(appointments, searchModel.AppointmentDate);
                if (appointments != null && appointments.Count > 0)
                    return appointments.ToList(); ;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        private ICollection<Appointment> SearchByDate(ICollection<Appointment> appointments, DateTime? dateTime)
        {
            if (dateTime == null || appointments.Count == 0 || appointments == null)
            {
                return appointments;
            }
            else
            {
                return appointments.Where(e => e.AppointmentDate == dateTime).ToList();
            }
        }

        private ICollection<Appointment> SeachByAge(ICollection<Appointment> appointments, Range<int>? age)
        {
            if (age == null || appointments.Count == 0 || appointments == null)
            {
                return appointments;
            }
            else
            {
                return appointments.Where(e => e.PatientAge >= age.MinAge && e.PatientAge <= age.MaxAge).ToList();
            }
        }

        private ICollection<Appointment> SearchByName(ICollection<Appointment> appointments, string? name)
        {
            if (name == null || appointments == null || appointments.Count == 0)
            {
                return appointments;
            }
            else
            {
                return appointments.Where(e => e.PatientName.ToLower().Contains(name.ToLower())).ToList();
            }
        }

        
    }
}