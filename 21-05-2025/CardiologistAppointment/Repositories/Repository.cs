using CardiologistAppointment.Interfaces;
using CardiologistAppointment.Exceptions;

namespace CardiologistAppointment.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected List<T> _items = new List<T>();
        protected abstract K GenerateID();
        public abstract ICollection<T> GetAll();
        public abstract T GetById(K id);

        public T Add(T item)
        {
            var id = GenerateID();
            var property = typeof(T).GetProperty("Id");
            if (property != null)
            {
                property.SetValue(item, id);
            }
            if (_items.Contains(item))
            {
                throw new DuplicateEntityException("Appointment already exists");
            }
            _items.Add(item);
            return item;
        }

    }

}

