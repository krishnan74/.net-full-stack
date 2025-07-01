using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public  class TeacherSubjectRepository : Repository<long, TeacherSubject>
    {
        public TeacherSubjectRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<TeacherSubject> Get(long key)
        {
            var teacherSubject = await _quizContext.teacherSubjects.Include(ts => ts.Teacher)
                .Include(ts => ts.Subject)
                .SingleOrDefaultAsync(p => p.Id == key);

            return teacherSubject??throw new KeyNotFoundException($"No teacherSubject with the given ID: {key}");
        }

        public override async Task<IEnumerable<TeacherSubject>> GetAll()
        {
            var teacherSubjects = _quizContext.teacherSubjects.Include(ts => ts.Teacher)
                .Include(ts => ts.Subject);
            if (teacherSubjects.Count() == 0)
                return Enumerable.Empty<TeacherSubject>();
            return (await teacherSubjects.ToListAsync());
        }
    }
}