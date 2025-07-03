using QuizupAPI.Models;
namespace QuizupAPI.Interfaces
{
    public interface IClassService
    {
        Task<Class> AddClassAsync(string className, ICollection<long>? subjectIds = null);
        Task<Class> UpdateClassAsync(long id, string className);
        Task<Class> DeleteClassAsync(long id);
        Task<Class> GetClassByIdAsync(long id);
        Task<IEnumerable<Class>> GetAllClassesAsync();
        Task<IEnumerable<Subject>> GetSubjectsByClassIdAsync(long classId);
        Task<Class> AddSubjectToClassAsync(long classId, long subjectId);

    }
}