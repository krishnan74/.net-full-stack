using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Contexts;
using QuizupAPI.Models.DTOs.Subject;
using QuizupAPI.Misc.Mappers;

namespace QuizupAPI.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IRepository<long, Subject> _subjectRepository;

        public SubjectMapper subjectMapper;
        private readonly QuizContext _context;
        public SubjectService(IRepository<long, Subject> subjectRepository, QuizContext context)
        {
            _subjectRepository = subjectRepository;
            _context = context;
        }

        public async Task<Subject> AddSubjectAsync(SubjectDTO subjectDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subjectDTO.Name) || string.IsNullOrWhiteSpace(subjectDTO.Code))
                {
                    throw new ArgumentException("Subject name and code cannot be null or empty.");
                }

                var newSubject = subjectMapper.MapSubjectAddRequestSubject(subjectDTO);

                if (newSubject == null)
                {
                    throw new Exception("Failed to map SubjectDTO to Subject.");
                }

                var addedSubject = await _subjectRepository.Add(newSubject);

                if (addedSubject == null)
                {
                    throw new Exception("Failed to add new subject.");
                }

                return addedSubject;

            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Invalid subject data: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the subject.", ex);
            }
        }

        public async Task<Subject> UpdateSubjectAsync(long id, SubjectDTO subjectDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subjectDTO.Name) || string.IsNullOrWhiteSpace(subjectDTO.Code))
                {
                    throw new ArgumentException("Subject name and code cannot be null or empty.");
                }

                var existingSubject = await _subjectRepository.Get(id);

                var mappedSubject = subjectMapper.MapSubjectUpdateRequestSubject(existingSubject, subjectDTO);
                if (mappedSubject == null)
                {
                    throw new Exception("Failed to map SubjectDTO to existing Subject.");
                }

                var updatedSubject = await _subjectRepository.Update(id, mappedSubject);

                if (updatedSubject == null)
                {
                    throw new Exception("Failed to update subject.");
                }

                return updatedSubject;

            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Invalid subject data: {ex.Message}", ex);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the subject.", ex);
            }
        }

        public async Task<Subject> DeleteSubjectAsync(long id)
        {
            try
            {
                var existingSubject = await _subjectRepository.Get(id);

                var deletedSubject = await _subjectRepository.Delete(id);

                return deletedSubject;
            }
            catch(
                KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"No subject found with ID: {id}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the subject with ID {id}.", ex);
            }
            
        }

        public async Task<Subject> GetSubjectByIdAsync(long id)
        {
            try
            {
                return await _subjectRepository.Get(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Subject with ID {id} not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the subject with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
             try
            {
                return await _subjectRepository.GetAll();

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all subjects.", ex);
            }
        }
    
    }
}