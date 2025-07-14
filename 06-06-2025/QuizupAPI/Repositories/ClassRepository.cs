using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public  class ClassRepository : Repository<long, Classe>
    {
        public ClassRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<Classe> Get(long key)
        {
            var classe = await _quizContext.classes
                .Include(cl => cl.Quizzes)
                .ThenInclude(q => q.QuizQuestions)
                .Include(cl => cl.Quizzes)
                .ThenInclude(q => q.Teacher)
                .Include(cl => cl.Quizzes)
                .ThenInclude(q => q.Subject)
                
                .SingleOrDefaultAsync(p => p.Id == key);

            return classe??throw new KeyNotFoundException($"No class with the given ID: {key}");
        }

        public override async Task<IEnumerable<Classe>> GetAll()
        {
            var classes = _quizContext.classes.Include(c => c.ClassSubjects).ThenInclude(cs => cs.Subject);
                
            if (classes.Count() == 0)
                return Enumerable.Empty<Classe>();
            return (await classes.ToListAsync());
        }
    }
}