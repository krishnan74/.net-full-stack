using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public  class StudentRepository : Repository<long, Student>
    {
        public StudentRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<Student> Get(long key)
        {
            var student = await _quizContext.students.SingleOrDefaultAsync(p => p.Id == key);

            return student??throw new Exception("No student with the given ID");
        }

        public override async Task<IEnumerable<Student>> GetAll()
        {
            var students = _quizContext.students.Where(s => !s.IsDeleted);
            if (!students.Any())
                throw new Exception("No active students in the database");
            return (await students.ToListAsync());
        }
    }
}