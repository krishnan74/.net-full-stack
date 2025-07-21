
using VideoPortalAPI.Contexts;
using VideoPortalAPI.Interfaces;

namespace VideoPortalAPI.Repositories
{
    public  abstract class Repository<K, T> : IRepository<K, T> where T:class
    {
        protected readonly VideoPortalContext _videoPortalContext;

        public Repository(VideoPortalContext videoPortalContext)
        {
            _videoPortalContext = videoPortalContext;
        }

        public async Task<T> Add(T item)
        {
            _videoPortalContext.Add(item);
            await _videoPortalContext.SaveChangesAsync();
            return item;
        }

        public abstract Task<T> GetById(K key);

        public abstract Task<IEnumerable<T>> GetAll();

        public async Task<T> Update(K key, T item)
        {
            var myItem = await GetById(key);
            if (myItem != null)
            {
                _videoPortalContext.Entry(myItem).CurrentValues.SetValues(item);
                await _videoPortalContext.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for updation");
        }

        public async Task<T> Delete(K key)
        {
            var item = await GetById(key);
            if (item != null)
            {
                _videoPortalContext.Remove(item);
                await _videoPortalContext.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for deleting");
        }

    }
}
