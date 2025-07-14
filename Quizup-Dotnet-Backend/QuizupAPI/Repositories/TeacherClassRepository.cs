using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public  class TeacherClassRepository : Repository<long, TeacherClass>
    {
        public TeacherClassRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<TeacherClass> Get(long key)
        {
            var teacherClass = await _quizContext.teacherClasses.Include(tc => tc.Classe)
                .Include(tc => tc.Teacher)
                .SingleOrDefaultAsync(p => p.Id == key);

            return teacherClass??throw new KeyNotFoundException($"No teacherClass with the given ID: {key}");
        }

        public override async Task<IEnumerable<TeacherClass>> GetAll()
        {
            var teacherClasses = _quizContext.teacherClasses.Include(tc => tc.Classe)
                .Include(tc => tc.Teacher);
            if (teacherClasses.Count() == 0)
                return Enumerable.Empty<TeacherClass>();
            return (await teacherClasses.ToListAsync());
        }
    }
}