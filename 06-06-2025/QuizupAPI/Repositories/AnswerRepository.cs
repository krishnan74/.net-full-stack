using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public  class AnswerRepository : Repository<long, Answer>
    {
        public AnswerRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<Answer> Get(long key)
        {
            var answer = await _quizContext.answers.SingleOrDefaultAsync(p => p.Id == key);

            return answer??throw new Exception("No answer with the given ID");
        }

        public override async Task<IEnumerable<Answer>> GetAll()
        {
            var answers = _quizContext.answers;
            if (answers.Count() == 0)
                throw new Exception("No answer in the database");
            return (await answers.ToListAsync());
        }
    }
}