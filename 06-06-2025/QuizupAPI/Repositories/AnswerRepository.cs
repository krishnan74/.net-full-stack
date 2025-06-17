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

            return answer??throw new KeyNotFoundException($"No answer with the given ID: {key}");
        }

        public override async Task<IEnumerable<Answer>> GetAll()
        {
            var answers = _quizContext.answers;
            if (answers.Count() == 0)
                return Enumerable.Empty<Answer>();
            return (await answers.ToListAsync());
        }
    }
}