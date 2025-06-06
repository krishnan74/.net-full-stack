using QuizupAPI.Models.DTOs.Teacher;
using QuizupAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizupAPI.Interfaces
{
    public interface ITeacherService
    {
        Task<Teacher> AddTeacherAsync(TeacherAddRequestDTO teacher);
        Task<Teacher> GetTeacherByIdAsync(int id);
        Task<IEnumerable<Teacher>> GetAllTeachersAsync();
        Task<Teacher> UpdateTeacherAsync(int id, TeacherUpdateRequestDTO teacher);
        Task<Teacher> DeleteTeacherAsync(int id);
        Task<Quiz> StartQuizAsync(int teacherId, int quizId);
        Task<Quiz> EndQuizAsync(int teacherId, int quizId);
    }
}