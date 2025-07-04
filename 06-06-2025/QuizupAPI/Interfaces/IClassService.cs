using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Classe;

namespace QuizupAPI.Interfaces
{
    public interface IClassService
    {
        Task<Classe> AddClassAsync(string className, ICollection<long>? subjectIds = null);
        Task<Classe> UpdateClassAsync(long id, ClassUpdateDTO classUpdateDTO);
        Task<Classe> DeleteClassAsync(long id);
        Task<Classe> GetClassByIdAsync(long id);
        Task<IEnumerable<Classe>> GetAllClassesAsync();
        Task<IEnumerable<Subject>> GetSubjectsByClassIdAsync(long classId);
        Task<Classe> AddSubjectsToClassAsync(long classId, ICollection<long> subjectIds);
        Task<Classe> RemoveSubjectsFromClassAsync(long classId, ICollection<long> subjectIds);

    }
}