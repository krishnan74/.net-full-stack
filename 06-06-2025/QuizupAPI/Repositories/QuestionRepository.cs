using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public  class QuestionRepository : Repository<long, Question>
    {
        public QuestionRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<Question> Get(long key)
        {
            var question = await _quizContext.questions.SingleOrDefaultAsync(p => p.Id == key);

            return question??throw new Exception("No question with the given ID");
        }

        public override async Task<IEnumerable<Question>> GetAll()
        {
            var questions = _quizContext.questions;
            if (questions.Count() == 0)
                return Enumerable.Empty<Question>();
            return (await questions.ToListAsync());
        }
    }
}