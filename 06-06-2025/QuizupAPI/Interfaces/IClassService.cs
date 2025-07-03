using QuizupAPI.Models;
namespace QuizupAPI.Interfaces
{
    public interface IClassService
    {
        Task<Classe> AddClassAsync(string className, ICollection<long>? subjectIds = null);
        Task<Classe> UpdateClassAsync(long id, string className);
        Task<Classe> DeleteClassAsync(long id);
        Task<Classe> GetClassByIdAsync(long id);
        Task<IEnumerable<Classe>> GetAllClassesAsync();
        Task<IEnumerable<Subject>> GetSubjectsByClassIdAsync(long classId);
        Task<Classe> AddSubjectToClassAsync(long classId, long subjectId);

    }
}