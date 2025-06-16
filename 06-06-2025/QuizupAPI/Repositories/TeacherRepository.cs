using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public  class TeacherRepository : Repository<long, Teacher>
    {
        public TeacherRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<Teacher> Get(long key)
        {
            var teacher = await _quizContext.teachers.SingleOrDefaultAsync(p => p.Id == key);

            return teacher??throw new Exception("No teacher with the given ID");
        }

        public override async Task<IEnumerable<Teacher>> GetAll()
        {
            var teachers = _quizContext.teachers.Where(t => !t.IsDeleted);
            if (!teachers.Any())
                return Enumerable.Empty<Teacher>();
            return (await teachers.ToListAsync());
        }
    }
}