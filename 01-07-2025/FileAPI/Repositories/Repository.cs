
using FileAPI.Contexts;
using FileAPI.Interfaces;

namespace FileAPI.Repositories
{
    public  abstract class Repository<K, T> : IRepository<K, T> where T:class
    {
        protected readonly FileManagerContext _fileManagerContext;

        public Repository(FileManagerContext fileManagerContext)
        {
            _fileManagerContext = fileManagerContext;
        }

        public async Task<T> Add(T item)
        {
            _fileManagerContext.Add(item);
            await _fileManagerContext.SaveChangesAsync(); //generate and execute the DML quries for the objects whse state is in ['added','modified','deleted'],
            return item;
        }

        public abstract Task<T> Get(K key);

        public abstract Task<IEnumerable<T>> GetAll();

        public async Task<T> Update(K key, T item)
        {
            var myItem = await Get(key);
            if (myItem != null)
            {
                _fileManagerContext.Entry(myItem).CurrentValues.SetValues(item);
                await _fileManagerContext.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for updation");
        }

        public async Task<T> Delete(K key)
        {
            var item = await Get(key);
            if (item != null)
            {
                _fileManagerContext.Remove(item);
                await _fileManagerContext.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for deleting");
        }

    }
}
