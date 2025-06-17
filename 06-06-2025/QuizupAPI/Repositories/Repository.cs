
using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;

namespace QuizupAPI.Repositories
{
    public  abstract class Repository<K, T> : IRepository<K, T> where T:class
    {
        protected readonly QuizContext _quizContext;

        public Repository(QuizContext quizContext)
        {
            _quizContext = quizContext;
        }

        public async Task<T> Add(T item)
        {
            _quizContext.Add(item);
            await _quizContext.SaveChangesAsync(); //generate and execute the DML quries for the objects whse state is in ['added','modified','deleted'],
            return item;
        }

        public abstract Task<T> Get(K key);

        public abstract Task<IEnumerable<T>> GetAll();

        public async Task<T> Update(K key, T item)
        {
            var myItem = await Get(key);
            if (myItem != null)
            {
                _quizContext.Entry(myItem).CurrentValues.SetValues(item);
                await _quizContext.SaveChangesAsync();
                return item;
            }
            throw new KeyNotFoundException("No such item found for updation");
        }

        public async Task<T> Delete(K key)
        {
            var item = await Get(key);
            if (item != null)
            {
                _quizContext.Remove(item);
                await _quizContext.SaveChangesAsync();
                return item;
            }
            throw new KeyNotFoundException("No such item found for deleting");
        }

    }
}
