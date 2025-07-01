using QuizupAPI.Models;
namespace QuizupAPI.Interfaces
{
    public interface IClassService
    {
        Task<Class> AddClassAsync(Class classe);
        Task<Class> UpdateClassAsync(long id, Class classe);
        Task<Class> DeleteClassAsync(long id);
        Task<Class> GetClassByIdAsync(long id);
        Task<IEnumerable<Class>> GetAllClassesAsync();
        Task<IEnumerable<Subject>> GetSubjectsByClassIdAsync(long classId);

    }
}