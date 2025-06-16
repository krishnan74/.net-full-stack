using QuizupAPI.Models.DTOs.Quiz;
using QuizupAPI.Models.DTOs.Question;
using QuizupAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizupAPI.Interfaces
{
    public interface IQuizService
    {
        Task<Quiz> AddQuizAsync(QuizAddRequestDTO quiz);
        Task<Quiz> GetQuizByIdAsync(long id);
        Task<IEnumerable<Quiz>> GetAllQuizzesAsync();
        Task<Quiz> UpdateQuizAsync(long quizId, long teacherId, QuizUpdateRequestDTO quiz);
        Task<Question> AddQuestionToQuizAsync(long quizId, long teacherId, QuestionAddRequestDTO question);
        Task<Question> UpdateQuestionAsync(long quizId, long teacherId, QuestionUpdateRequestDTO question);
        Task<Quiz> DeleteQuizAsync(long id, long teacherId);

        Task<IEnumerable<Quiz>> GetQuizzesPaginationAsync(
            int pageNumber,
            int pageSize
            );
    }
}