using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public class TeacherRepository : Repository<long, Teacher>
    {
        public TeacherRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<Teacher> Get(long key)
        {
            var teacher = await _quizContext.teachers
                .Include(t => t.Quizzes)
          
                .SingleOrDefaultAsync(p => p.Id == key);

            return teacher ?? throw new KeyNotFoundException($"No teacher with the given ID: {key}");
        }

        public override async Task<IEnumerable<Teacher>> GetAll()
        {
            var teachers = _quizContext.teachers.Include(t => t.TeacherClasses).ThenInclude(tc => tc.Classe)
                .Include(t => t.TeacherSubjects).ThenInclude(ts => ts.Subject);
            if (!teachers.Any())
                return Enumerable.Empty<Teacher>();
            return (await teachers.ToListAsync());
        }


        public override async Task<Teacher> Delete(long key)
        {
            var teacher = await Get(key);
            if (teacher != null)
            {
                teacher.IsDeleted = true;
                await _quizContext.SaveChangesAsync();
                return teacher;
            }
            throw new KeyNotFoundException("No such teacher found for deletion");
        }
        
    }
}