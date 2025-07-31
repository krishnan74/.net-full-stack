
using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;

namespace DotnetAPI.Repositories
{
    public  abstract class Repository<K, T> : IRepository<K, T> where T:class
    {
        protected readonly DotnetAPIContext _context;

        public Repository(DotnetAPIContext DotnetAPIContext)
        {
            _context = DotnetAPIContext;
        }

        public async Task<T> Add(T item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync(); 
            return item;
        }

        public abstract Task<T> Get(K key);

        public abstract Task<IEnumerable<T>> GetAll(
            int pageNumber = 1,
            int pageSize = 10
        );

        public async Task<T> Update(K key, T item)
        {
            var myItem = await Get(key);
            if (myItem != null)
            {
                _context.Entry(myItem).CurrentValues.SetValues(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new KeyNotFoundException("No such item found for updation");
        }

        public virtual async Task<T> Delete(K key)
        {
            var item = await Get(key);
            if (item != null)
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new KeyNotFoundException("No such item found for deleting");
        }

    }
}
