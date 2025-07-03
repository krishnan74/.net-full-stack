using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Contexts;
using QuizupAPI.Models.DTOs.Classe;

namespace QuizupAPI.Services
{
    public class ClassService : IClassService
    {
        private readonly IRepository<long, Classe> _classeRepository;

        private readonly IRepository<long, ClassSubject> _classSubjectRepository;
        private readonly IRepository<long, Subject> _subjectRepository;
        private readonly QuizContext _context;
        public ClassService(IRepository<long, Classe> classeRepository, IRepository<long, ClassSubject> classSubjectRepository, IRepository<long, Subject> subjectRepository, QuizContext context)
        {
            _classeRepository = classeRepository;
            _classSubjectRepository = classSubjectRepository;
            _subjectRepository = subjectRepository;
            _context = context;
        }

        public async Task<Classe> AddClassAsync(string className, ICollection<long>? subjectIds = null)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(className))
                {
                    throw new ArgumentException("Class name cannot be null or empty.");
                }

                var mappedClass = new Classe { Name = className, UpdatedAt = DateTime.UtcNow };

                var addedClass = await _classeRepository.Add(mappedClass);

                if (subjectIds != null && subjectIds.Any())
                {
                    await AddSubjectsToClassAsync(addedClass.Id, subjectIds);
                }
                return addedClass;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Invalid class data: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                throw new Exception("An error occurred while adding the class.", ex);
            }
        }

        public async Task<Classe> UpdateClassAsync(long id, ClassUpdateDTO classUpdateDTO)
        {
            try
            {
                if (classUpdateDTO == null || string.IsNullOrWhiteSpace(classUpdateDTO.ClassName))
                {
                    throw new ArgumentException("Class name cannot be null or empty.");
                }
                var classe = await _classeRepository.Get(id);
                if (classe == null)
                {
                    throw new ArgumentException("Class not found.");
                }
                classe.Name = classUpdateDTO.ClassName;
                classe.UpdatedAt = DateTime.UtcNow;

                var updatedClass = await _classeRepository.Update(id, classe);

                if (updatedClass == null)
                {
                    throw new Exception($"Failed to update class:");
                }

                if (classUpdateDTO.RemoveSubjectIds != null && classUpdateDTO.RemoveSubjectIds.Any())
                {
                    await RemoveSubjectsFromClassAsync(updatedClass.Id, classUpdateDTO.RemoveSubjectIds);
                }

                if (classUpdateDTO.AddSubjectIds != null && classUpdateDTO.AddSubjectIds.Any())
                {
                    await AddSubjectsToClassAsync(updatedClass.Id, classUpdateDTO.AddSubjectIds);
                }

               

                return updatedClass;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Invalid class data: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the class.", ex);
            }
        }

        public async Task<Classe> DeleteClassAsync(long id)
        {
            try
            {
                var classe = await _classeRepository.Get(id);
                var deletedClass = await _classeRepository.Delete(id);
                return deletedClass;

            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }

            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the class with ID {id}.", ex);
            }
        }

        public async Task<Classe> GetClassByIdAsync(long id)
        {
            try
            {
                return await _classeRepository.Get(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Class with ID {id} not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the class with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Classe>> GetAllClassesAsync()
        {
            try
            {
                return await _classeRepository.GetAll();

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all classes.", ex);
            }
        }

        public async Task<IEnumerable<Subject>> GetSubjectsByClassIdAsync(long classId)
        {
            try
            {
                var classe = await _classeRepository.Get(classId);

                var classSubjects = await _classSubjectRepository.GetAll();
                return classSubjects.Where(cs => cs.ClassId == classId).Select(cs => cs.Subject).ToList();

            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving subjects for class with ID {classId}.", ex);
            }
        }

        public async Task<Classe> AddSubjectsToClassAsync(long classId, ICollection<long> subjectIds)
        {
            try
            {
                var classe = await _classeRepository.Get(classId);

                foreach (var subjectId in subjectIds)
                {
                    var existingSubject = await _subjectRepository.Get(subjectId);

                    var classSubject = new ClassSubject
                    {
                        ClassId = classId,
                        SubjectId = subjectId
                    };

                    await _classSubjectRepository.Add(classSubject);
                }

                return classe;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding subjects to the class.", ex);
            }
        }
        
        public async Task<Classe> RemoveSubjectsFromClassAsync(long classId, ICollection<long> subjectIds)
        {
            try
            {
                var classe = await _classeRepository.Get(classId);

                foreach (var subjectId in subjectIds)
                {
                    var classSubject = await _classSubjectRepository.GetAll();
                    var existingClassSubject = classSubject.FirstOrDefault(cs => cs.ClassId == classId && cs.SubjectId == subjectId);
                    if (existingClassSubject != null)
                    {
                        await _classSubjectRepository.Delete(existingClassSubject.Id);
                    }
                }

                return classe;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing subjects from the class.", ex);
            }
        }
     
    }
}