using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public  class SubjectRepository : Repository<long, Subject>
    {
        public SubjectRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<Subject> Get(long key)
        {
            var quizQuestion = await _quizContext.quizQuestions.SingleOrDefaultAsync(p => p.Id == key);

            return quizQuestion??throw new KeyNotFoundException($"No quizQuestion with the given ID: {key}");
        }

        public override async Task<IEnumerable<Subject>> GetAll()
        {
            var quizQuestions = _quizContext.quizQuestions;
            if (quizQuestions.Count() == 0)
                return Enumerable.Empty<Subject>();
            return (await quizQuestions.ToListAsync());
        }
    }
}