using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Repositories;

namespace FirstAPI.Services
{
    public class AppointmentService : IAppointmentService
    {
        IRepository<int, Appointment> _appointmentRepository;
        
        public AppointmentService(IRepository<int, Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public Appointment GetAppointmentById(int id)
        {
            try
            {
                var appointment = _appointmentRepository.GetById(id);
                if (appointment != null)
                {
                    return appointment;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public async Task<Appointment> AddAppointment(Appointment appointment)
        {
            try
            {
                var result = await _appointmentRepository.Add(appointment);
                if (result != null)
                {
                    return result.AppointmentNumber;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
           
        }

        public Appointment UpdateAppointment(Appointment appointment)
        {
            try
            {
                var result = _appointmentRepository.Update(appointment);
                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public void DeleteAppointment(int id)
        {
            try
            {
                _appointmentRepository.Delete(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<Appointment>? SearchAppointment(AppointmentSearchModel searchModel)
        {
            try
            {
                var appointments = _appointmentRepository.GetAll();
                appointments = SearchByPatientName(appointments, searchModel.PatientName);
                appointments = SeachByAge(appointments, searchModel.AgeRange);
                appointments = SearchByDate(appointments, searchModel.AppointmentDate);
                appointments = SearchByDoctorName(appointments, searchModel.DoctorName);
                appointments = SearchByDiagnosis(appointments, searchModel.Diagnosis);
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
                return appointments.Where(e => e.AppointmentDateTime == dateTime).ToList();
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
                return appointments.Where(e => e.Patient.Age >= age.MinAge && e.Patient.Age <= age.MaxAge).ToList();
            }
        }

        private ICollection<Appointment> SearchByPatientName(ICollection<Appointment> appointments, string? name)
        {
            if (name == null || appointments == null || appointments.Count == 0)
            {
                return appointments;
            }
            else
            {
                return appointments.Where(e => e.Patient.Name.ToLower().Contains(name.ToLower())).ToList();
            }
        }

        private ICollection<Appointment> SearchByDoctorName(ICollection<Appointment> appointments, string? doctorName)
        {
            if (doctorName == null || appointments == null || appointments.Count == 0)
            {
                return appointments;
            }
            else
            {
                return appointments.Where(e => e.Doctor.Name.ToLower().Contains(doctorName.ToLower())).ToList();
            }
        }

        private ICollection<Appointment> SearchByDiagnosis(ICollection<Appointment> appointments, string? diagnosis)
        {
            if (diagnosis == null || appointments == null || appointments.Count == 0)
            {
                return appointments;
            }
            else
            {
                return appointments.Where(e => e.Patient.Diagnosis.ToLower().Contains(diagnosis.ToLower())).ToList();
            }
        }

        
    }
}