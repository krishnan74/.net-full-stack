using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public  class QuizRepository : Repository<long, Quiz>
    {
        public QuizRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<Quiz> Get(long key)
        {
            var quiz = await _quizContext.quizzes.SingleOrDefaultAsync(p => p.Id == key);

            return quiz??throw new Exception("No Quiz with the given ID");
        }

        public override async Task<IEnumerable<Quiz>> GetAll()
        {
            var quizzes = _quizContext.quizzes;
            if (quizzes.Count() == 0)
                throw new Exception("No Quiz in the database");
            return (await quizzes.ToListAsync());
        }
    }
}