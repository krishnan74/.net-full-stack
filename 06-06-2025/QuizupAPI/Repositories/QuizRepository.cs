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
            var quiz = await _quizContext.quizzes
                                .Include(q => q.Teacher)
                                .Include(q => q.QuizQuestions).ThenInclude(q => q.Question)
                                .Include(q => q.Subject)
                                .Include(q => q.Classe)
                                .Include(q => q.Submissions).ThenInclude(qs => qs.Student)
                                .SingleOrDefaultAsync(p => p.Id == key );

            return quiz??throw new KeyNotFoundException($"No Quiz with the given ID: {key}");
        }

        public override async Task<IEnumerable<Quiz>> GetAll()
        {
            var quizzes = _quizContext.quizzes.Include(q => q.Teacher).Include(q => q.QuizQuestions);
            if (!quizzes.Any())
                return Enumerable.Empty<Quiz>();
            return (await quizzes.ToListAsync());
        }
    }
}