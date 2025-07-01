using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Contexts;


namespace QuizupAPI.Services
{
    public class ClassService : IClassService
    {
        private readonly IRepository<long, Class> _classeRepository;

        private readonly IRepository<long, ClassSubject> _classSubjectRepository;
        private readonly IRepository<long, Subject> _subjectRepository;
        private readonly QuizContext _context;
        public ClassService(IRepository<long, Class> classeRepository, IRepository<long, ClassSubject> classSubjectRepository, IRepository<long, Subject> subjectRepository, QuizContext context)
        {
            _classeRepository = classeRepository;
            _classSubjectRepository = classSubjectRepository;
            _subjectRepository = subjectRepository;
            _context = context;
        }

        public async Task<Class> AddClassAsync(string className, ICollection<long>? subjectIds = null)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(className))
                {
                    throw new ArgumentException("Class name cannot be null or empty.");
                }

                var mappedClass = new Class { Name = className, UpdatedAt = DateTime.UtcNow };

                var addedClass = await _classeRepository.Add(mappedClass);

                if (subjectIds != null && subjectIds.Any())
                {
                    foreach (var subjectId in subjectIds)
                    {
                        await AddSubjectToClassAsync(addedClass.Id, subjectId);
                    }
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

        public async Task<Class> UpdateClassAsync(long id, string className)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(className))
                {
                    throw new ArgumentException("Class name cannot be null or empty.");
                }
                var classe = await _classeRepository.Get(id);
                if (classe == null)
                {
                    throw new ArgumentException("Class not found.");
                }
                classe.Name = className;
                classe.UpdatedAt = DateTime.UtcNow;

                return await _classeRepository.Update(id, classe);
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

        public async Task<Class> DeleteClassAsync(long id)
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

        public async Task<Class> GetClassByIdAsync(long id)
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

        public async Task<IEnumerable<Class>> GetAllClassesAsync()
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

        public async Task<Class> AddSubjectToClassAsync(long classId, long subjectId)
        {
            try
            {
                var classe = await _classeRepository.Get(classId);

                var existingSubject = await _subjectRepository.Get(subjectId);

                var classSubject = new ClassSubject
                {
                    ClassId = classId,
                    SubjectId = subjectId
                };

                var createdClassSubject = await _classSubjectRepository.Add(classSubject);

                if (createdClassSubject == null)
                {
                    throw new Exception("Failed to add subject to class.");
                }

                return classe;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the subject to the class.", ex);
            }
        }

      
    }
}