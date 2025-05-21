using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardiologistAppointment.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        T Add(T item);
        T GetById(K id);
        ICollection<T> GetAll();
    }
}
