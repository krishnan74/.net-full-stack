using CardiologistAppointment.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardiologistAppointment.Exceptions;
using CardiologistAppointment.Models;

namespace CardiologistAppointment.Repositories
{
    public class AppointmentRepository : Repository<int, Appointment>
    {
        public AppointmentRepository() : base()
        {
        }
        public override ICollection<Appointment> GetAll()
        {
            if (_items.Count == 0)
            {
                throw new CollectionEmptyException("No appointments found");
            }
            return _items;
        }

        public override Appointment GetById(int id)
        {
            var employee = _items.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                throw new KeyNotFoundException("Appointment not found");
            }
            return employee;
        }

        protected override int GenerateID()
        {
            if (_items.Count == 0)
            {
                return 101;
            }
            else
            {
                return _items.Max(e => e.Id) + 1;
            }
        }
    }

}