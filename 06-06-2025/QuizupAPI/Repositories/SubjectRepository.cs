using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public class SubjectRepository : Repository<long, Subject>
    {
        public SubjectRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<Subject> Get(long key)
        {
            var subject = await _quizContext.subjects.SingleOrDefaultAsync(p => p.Id == key);

            return subject ?? throw new KeyNotFoundException($"No subject with the given ID: {key}");
        }

        public override async Task<IEnumerable<Subject>> GetAll()
        {
            var subjects = _quizContext.subjects;
            if (subjects.Count() == 0)
                return Enumerable.Empty<Subject>();
            return (await subjects.ToListAsync());
        }
        
        public override async Task<Subject> Delete(long key)
        {
            var subject = await Get(key);
            if (subject != null)
            {
                subject.IsDeleted = true;
                await _quizContext.SaveChangesAsync();
                return subject;
            }
            throw new KeyNotFoundException("No such subject found for deletion");
        }
    }
}