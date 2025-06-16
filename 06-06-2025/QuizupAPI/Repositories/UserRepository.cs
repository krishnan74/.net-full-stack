using QuizupAPI.Contexts;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Repositories
{
    public  class UserRepository : Repository<string, User>
    {
        public UserRepository(QuizContext quizContext) : base(quizContext)
        {
        }

        public override async Task<User> Get(string key)
        {
            var user = await _quizContext.users.SingleOrDefaultAsync(p => p.Username == key);

            return user??throw new Exception("No user with the given ID");
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            var users = _quizContext.users.Where(u => !u.IsDeleted);
            if (!users.Any())
                throw new Exception("No active users in the database");
            return (await users.ToListAsync());
        }
    }
}