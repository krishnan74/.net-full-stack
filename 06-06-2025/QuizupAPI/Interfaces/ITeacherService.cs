using QuizupAPI.Models.DTOs.Teacher;
using QuizupAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizupAPI.Interfaces
{
    public interface ITeacherService
    {
        Task<Teacher> AddTeacherAsync(TeacherAddRequestDTO teacher);
        Task<Teacher> GetTeacherByIdAsync(long id);
        Task<IEnumerable<Teacher>> GetAllTeachersAsync();
        Task<Teacher> UpdateTeacherAsync(long id, TeacherUpdateRequestDTO teacher);
        Task<Teacher> DeleteTeacherAsync(long id);
        Task<Quiz> StartQuizAsync(long teacherId, long quizId);
        Task<Quiz> EndQuizAsync(long teacherId, long quizId);
        Task<TeacherSummaryDTO> GetTeacherQuizSummaryAsync(long teacherId, DateTime? startDate = null, DateTime? endDate = null);
    }
}