using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Subject;
namespace QuizupAPI.Interfaces
{
    public interface ISubjectService
    {
        Task<Subject> AddSubjectAsync(SubjectDTO subject);
        Task<Subject> UpdateSubjectAsync(long id, SubjectDTO subject);
        Task<Subject> DeleteSubjectAsync(long id);
        Task<Subject> GetSubjectByIdAsync(long id);
        Task<IEnumerable<Subject>> GetAllSubjectsAsync();
    }
}