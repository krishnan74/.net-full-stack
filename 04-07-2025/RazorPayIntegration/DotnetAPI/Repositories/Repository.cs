
using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;

namespace DotnetAPI.Repositories
{
    public  abstract class Repository<K, T> : IRepository<K, T> where T:class
    {
        protected readonly DatabaseContext _databaseContext;

        public Repository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<T> Add(T item)
        {
            _databaseContext.Add(item);
            await _databaseContext.SaveChangesAsync(); 
            return item;
        }

        public abstract Task<T> Get(K key);

        public abstract Task<IEnumerable<T>> GetAll();

        public async Task<T> Update(K key, T item)
        {
            var myItem = await Get(key);
            if (myItem != null)
            {
                _databaseContext.Entry(myItem).CurrentValues.SetValues(item);
                await _databaseContext.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for updation");
        }

        public async Task<T> Delete(K key)
        {
            var item = await Get(key);
            if (item != null)
            {
                _databaseContext.Remove(item);
                await _databaseContext.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for deleting");
        }

    }
}
