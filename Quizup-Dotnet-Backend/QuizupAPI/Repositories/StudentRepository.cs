using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public class StudentRepository : Repository<long, Student>
    {
        public StudentRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<Student> Get(long key)
        {
            var student = await _quizContext.students.
                Include(s => s.QuizSubmissions).
                Include(s => s.Classe)
                .SingleOrDefaultAsync(p => p.Id == key);

            return student ?? throw new KeyNotFoundException($"No student with the given ID: {key}");
        }

        public override async Task<IEnumerable<Student>> GetAll()
        {
            var students = _quizContext.students.Include(s => s.Classe);
            if (!students.Any())
                return Enumerable.Empty<Student>();
            return (await students.ToListAsync());
        }

        public override async Task<Student> Delete(long key)
        {
            var student = await Get(key);
            if (student != null)
            {
                student.IsDeleted = true;
                await _quizContext.SaveChangesAsync();
                return student;
            }
            throw new KeyNotFoundException("No such student found for deletion");
        }
    }
}