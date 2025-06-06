using QuizupAPI.Models.DTOs.Quiz;
using QuizupAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizupAPI.Interfaces
{
    public interface IQuizService
    {
        Task<Quiz> AddQuizAsync(QuizAddRequestDTO quiz);
        Task<Quiz> GetQuizByIdAsync(int id);
        Task<IEnumerable<Quiz>> GetAllQuizzesAsync();
        Task<Quiz> UpdateQuizAsync(int quizId, int teacherId, QuizUpdateRequestDTO quiz);
        Task<Quiz> DeleteQuizAsync(int id, int teacherId);
    }
}