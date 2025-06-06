using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Student;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuizupAPI.Models.DTOs.QuizSubmission;

namespace QuizupAPI.Interfaces
{
    public interface IStudentService
    {
        Task<Student> AddStudentAsync(StudentAddRequestDTO student);
        Task<Student> GetStudentByIdAsync(int id);
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> UpdateStudentAsync(int id, StudentUpdateRequestDTO student);
        Task<Student> DeleteStudentAsync(int id);
        Task<IEnumerable<QuizSubmission>> GetSubmissionsByStudentIdAsync(int id);
        Task<QuizSubmission> SubmitQuizAsync(int studentId, QuizSubmissionDTO submission);
        Task<QuizSubmission> SaveAnswersAsync(int studentId, QuizSubmissionDTO submission);

    }
}