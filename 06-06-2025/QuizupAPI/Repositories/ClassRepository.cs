using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public  class ClassRepository : Repository<long, Class>
    {
        public ClassRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<Class> Get(long key)
        {
            var classe = await _quizContext.classes.SingleOrDefaultAsync(p => p.Id == key);

            return classe??throw new KeyNotFoundException($"No class with the given ID: {key}");
        }

        public override async Task<IEnumerable<Class>> GetAll()
        {
            var classes = _quizContext.classes;
            if (classes.Count() == 0)
                return Enumerable.Empty<Class>();
            return (await classes.ToListAsync());
        }
    }
}