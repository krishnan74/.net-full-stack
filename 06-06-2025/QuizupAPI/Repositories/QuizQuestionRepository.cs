using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public  class QuizQuestionRepository : Repository<long, QuizQuestion>
    {
        public QuizQuestionRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<QuizQuestion> Get(long key)
        {
            var quizQuestion = await _quizContext.quizQuestions.SingleOrDefaultAsync(p => p.Id == key);

            return quizQuestion??throw new Exception("No quizQuestion with the given ID");
        }

        public override async Task<IEnumerable<QuizQuestion>> GetAll()
        {
            var quizQuestions = _quizContext.quizQuestions;
            if (quizQuestions.Count() == 0)
                throw new Exception("No quizQuestion in the database");
            return (await quizQuestions.ToListAsync());
        }
    }
}