using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public  class QuizSubmissionRepository : Repository<long, QuizSubmission>
    {
        public QuizSubmissionRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<QuizSubmission> Get(long key)
        {
            var quizSubmission = await _quizContext.quizSubmissions
                .Include(qs => qs.Answers)
                .Include(qs => qs.Student) 
                .SingleOrDefaultAsync(qs => qs.Id == key);

            return quizSubmission??throw new KeyNotFoundException($"No QuizSubmission with the given ID: {key}");
        }

        public override async Task<IEnumerable<QuizSubmission>> GetAll()
        {
            var quizzes = _quizContext.quizSubmissions.
                Include(qsub => qsub.Answers);
            if (quizzes.Count() == 0)
                return Enumerable.Empty<QuizSubmission>();
            return (await quizzes.ToListAsync());
        }
    }
}