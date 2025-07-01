using QuizupAPI.Models;
namespace QuizupAPI.Interfaces
{
    public interface ISubjectService
    {
        Task<Subject> AddSubjectAsync(Subject subject);
        Task<Subject> UpdateSubjectAsync(long id, Subject subject);
        Task<Subject> DeleteSubjectAsync(long id);
        Task<Subject> GetSubjectByIdAsync(long id);
        Task<IEnumerable<Subject>> GetAllSubjectsAsync();
    }
}