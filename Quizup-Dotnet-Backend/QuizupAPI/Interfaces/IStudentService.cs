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
        Task<Student> GetStudentByIdAsync(long id);
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> UpdateStudentAsync(long id, StudentUpdateRequestDTO student);
        Task<Student> DeleteStudentAsync(long id);
        Task<IEnumerable<QuizSubmission>> GetSubmissionsByStudentIdAsync(long id);
        Task<IEnumerable<QuizSubmission>> GetQuizSubmissionsByStudentIdAsync(long studentId, long quizId);
        Task<QuizSubmission> StartQuizAsync(long studentId, long quizId);
        Task<QuizSubmission> SubmitQuizAsync(long studentId, long quizSubmissionId, QuizSubmissionDTO submission);
        Task<QuizSubmission> SaveAnswersAsync(long studentId, long quizSubmissionId, QuizSubmissionDTO submission);
        Task<StudentSummaryDTO> GetStudentQuizSummaryAsync(long studentId, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<Student>> GetStudentsPaginationAsync(
            int pageNumber,
            int pageSize
            );
        
        Task<IEnumerable<Subject>> GetSubjectsByStudentIdAsync(long studentId);
    }
}