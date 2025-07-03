using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public  class ClassSubjectRepository : Repository<long, ClassSubject>
    {
        public ClassSubjectRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<ClassSubject> Get(long key)
        {
            var classSubject = await _quizContext.classSubjects.Include(cs => cs.Class)
                .Include(cs => cs.Subject)
                .SingleOrDefaultAsync(p => p.Id == key);

            return classSubject??throw new KeyNotFoundException($"No class with the given ID: {key}");
        }

        public override async Task<IEnumerable<ClassSubject>> GetAll()
        {
            var classSubjects = _quizContext.classSubjects.Include(cs => cs.Class)
                .Include(cs => cs.Subject);
            if (classSubjects.Count() == 0)
                return Enumerable.Empty<ClassSubject>();
            return (await classSubjects.ToListAsync());
        }
    }
}