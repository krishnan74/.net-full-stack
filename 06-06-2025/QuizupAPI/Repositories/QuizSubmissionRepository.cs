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
            var quizSubmission = await _quizContext.quizSubmissions.SingleOrDefaultAsync(p => p.Id == key);

            return quizSubmission??throw new Exception("No QuizSubmission with the given ID");
        }

        public override async Task<IEnumerable<QuizSubmission>> GetAll()
        {
            var quizzes = _quizContext.quizSubmissions;
            if (quizzes.Count() == 0)
                return Enumerable.Empty<QuizSubmission>();
            return (await quizzes.ToListAsync());
        }
    }
}