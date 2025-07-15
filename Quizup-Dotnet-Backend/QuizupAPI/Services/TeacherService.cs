using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Repositories;
using QuizupAPI.Models.DTOs.Teacher;
using QuizupAPI.Models.DTOs.Notifications;
using QuizupAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using QuizupAPI.Misc.Mappers;
using System.ComponentModel.DataAnnotations;
using QuizupAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace QuizupAPI.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IRepository<long, Teacher> _teacherRepository;
        private readonly IRepository<long, Quiz> _quizRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<string, User> _userRepository;
        private readonly IHubContext<QuizNotificationHub> _hubContext;

        private readonly IRepository<long, TeacherSubject> _teacherSubjectRepository;
        private readonly IRepository<long, TeacherClass> _teacherClassRepository;

        private readonly IRepository<long, Classe> _classRepository;

        private readonly IRepository<long, Subject> _subjectRepository;
        private readonly QuizContext _context;

        public UserMapper userMapper;
        public TeacherMapper teacherMapper;
        public TeacherService(IRepository<long, Teacher> teacherRepository, IRepository<long, Quiz> quizRepository, IHubContext<QuizNotificationHub> hubContext, IEncryptionService encryptionService, IRepository<string, User> userRepository, IRepository<long, TeacherSubject> teacherSubjectRepository, IRepository<long, TeacherClass> teacherClassRepository, IRepository<long, Classe> classRepository, IRepository<long, Subject> subjectRepository, QuizContext context)
        {
            _quizRepository = quizRepository;
            _hubContext = hubContext;
            _teacherRepository = teacherRepository;
            _encryptionService = encryptionService;
            _userRepository = userRepository;
            _teacherSubjectRepository = teacherSubjectRepository;
            _teacherClassRepository = teacherClassRepository;
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _context = context;
            teacherMapper = new TeacherMapper();
            userMapper = new UserMapper();
        }

        public async Task<Teacher> AddTeacherAsync(TeacherAddRequestDTO teacherDTO)
        {
            try
            {
                if (teacherDTO == null)
                {
                    throw new ArgumentNullException(nameof(teacherDTO), "Teacher data cannot be null.");
                }
                if (string.IsNullOrWhiteSpace(teacherDTO.FirstName) || string.IsNullOrWhiteSpace(teacherDTO.LastName) || string.IsNullOrWhiteSpace(teacherDTO.Email) || string.IsNullOrWhiteSpace(teacherDTO.Password))
                {
                    throw new ArgumentException("First name, last name, email, and password are required fields.");
                }
                if (!new EmailAddressAttribute().IsValid(teacherDTO.Email))
                {
                    throw new ArgumentException("Invalid email format.");
                }

                var allTeachers = await _teacherRepository.GetAll();
                var existingTeacher = allTeachers.FirstOrDefault(t => t.Email == teacherDTO.Email);
                if (existingTeacher != null)
                {
                    throw new InvalidOperationException("A teacher with this email already exists.");
                }


                var user = userMapper.MapTeacherAddRequestUser(teacherDTO);

                if (user == null)
                {
                    throw new Exception("Failed to map TeacherAddRequestDTO to User.");
                }

                var hashedPassword = _encryptionService.HashPassword(teacherDTO.Password);

                user.HashedPassword = hashedPassword;

                user = await _userRepository.Add(user);

                if (user == null)
                {
                    throw new Exception("Failed to add user.");
                }

                
                var teacher = teacherMapper.MapTeacherAddRequestTeacher(teacherDTO);
                if (teacher == null)
                {
                    throw new Exception("Failed to map TeacherAddRequestDTO to Teacher.");
                }

                var addedTeacher = await _teacherRepository.Add(teacher);

                if (teacherDTO.ClassIds != null && teacherDTO.ClassIds.Any())
                {
                    await AddClassesToTeacherAsync(addedTeacher.Id, teacherDTO.ClassIds );
                }
                if (teacherDTO.SubjectIds != null && teacherDTO.SubjectIds.Any())
                {
                    await AddSubjectsToTeacherAsync(addedTeacher.Id, teacherDTO.SubjectIds);
                }

                return addedTeacher;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the teacher.", ex);
            }
        }

        public async Task<Teacher> GetTeacherByIdAsync(long id)
        {
            try
            {
                return await _teacherRepository.Get(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the teacher with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            try
            {
                return await _teacherRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all teachers.", ex);
            }
        }

        public async Task<Teacher> UpdateTeacherAsync(long id, TeacherUpdateRequestDTO teacherUpdateDTO)
        {
            try
            {
                if (teacherUpdateDTO == null)
                {
                    throw new ArgumentNullException(nameof(teacherUpdateDTO), "Teacher data cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(teacherUpdateDTO.FirstName) || string.IsNullOrWhiteSpace(teacherUpdateDTO.LastName) )
                {
                    throw new ArgumentException("First name and last name are required fields.");
                }

                var existingTeacher = await _teacherRepository.Get(id);

                var mappedTeacher = teacherMapper.MapTeacherUpdateRequestTeacher(existingTeacher, teacherUpdateDTO);
                if (mappedTeacher == null)
                {
                    throw new Exception("Failed to map TeacherUpdateRequestDTO to Teacher.");
                }

                var updatedTeacher = await _teacherRepository.Update(id, mappedTeacher);
                if (updatedTeacher == null)
                {
                    throw new Exception($"Failed to update teacher with ID {id}.");
                }

                if(teacherUpdateDTO.RemoveSubjectIds != null && teacherUpdateDTO.RemoveSubjectIds.Any())
                {
                    await RemoveSubjectsFromTeacherAsync(updatedTeacher.Id, teacherUpdateDTO.RemoveSubjectIds);
                }

                if(teacherUpdateDTO.AddSubjectIds != null && teacherUpdateDTO.AddSubjectIds.Any())
                {
                    await AddSubjectsToTeacherAsync(updatedTeacher.Id, teacherUpdateDTO.AddSubjectIds);
                }

                if (teacherUpdateDTO.RemoveClassIds != null && teacherUpdateDTO.RemoveClassIds.Any())
                {
                    await RemoveClassesFromTeacherAsync(updatedTeacher.Id, teacherUpdateDTO.RemoveClassIds);
                }

                if (teacherUpdateDTO.AddClassIds != null && teacherUpdateDTO.AddClassIds.Any())
                {
                    await AddClassesToTeacherAsync(updatedTeacher.Id, teacherUpdateDTO.AddClassIds);
                }

                return updatedTeacher;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the teacher with ID {id}.", ex);
            }
        }

        public async Task<Teacher> DeleteTeacherAsync(long id)
        {
            try
            {
                var existingTeacher = await _teacherRepository.Get(id);

                var teacherSubjects = await _teacherSubjectRepository.GetAll();
                var subjectsToDelete = teacherSubjects.Where(ts => ts.TeacherId == id).ToList();
                foreach (var teacherSubject in subjectsToDelete)
                {
                    await _teacherSubjectRepository.Delete(teacherSubject.Id);
                }

                var teacherClasses = await _teacherClassRepository.GetAll();
                var classesToDelete = teacherClasses.Where(tc => tc.TeacherId == id).ToList();
                foreach (var teacherClass in classesToDelete)
                {
                    await _teacherClassRepository.Delete(teacherClass.Id);
                }

                var deletedTeacher = await _teacherRepository.Delete(id);
                await _userRepository.Delete(existingTeacher.Email);
                return deletedTeacher;
            }

            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }

            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the teacher with ID {id}.", ex);
            }

        }

        public async Task<Quiz> StartQuizAsync(long teacherId, long quizId)
        {
            try
            {
                var quiz = await _quizRepository.Get(quizId);

                if (quiz.TeacherId != teacherId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to start this quiz.");
                }
                if (quiz.IsActive)
                {
                    throw new InvalidOperationException("Quiz is already active.");
                }
                quiz.IsActive = true;

                var quizNotificationDTO = new QuizNotificationDTO
                {
                    Id = quiz.Id,
                    Title = quiz.Title,
                    Description = quiz.Description,
                    CreatedAt = DateTime.UtcNow,
                    DueDate = quiz.DueDate,
                    TeacherName = quiz.Teacher.FirstName + " " + quiz.Teacher.LastName,
                    SubjectName = quiz.Subject?.Name ?? "No Subject",
                    SubjectCode = quiz.Subject?.Code ?? "No Code",
                    ClassName = quiz.Classe?.Name ?? "No Class",
                    Type = "start"
                };
                string classGroupName = $"class_{quiz.ClassId}";
                await _hubContext.Clients.Group(classGroupName).SendAsync("NotifyStartQuiz", quizNotificationDTO);

                return await _quizRepository.Update(quizId, quiz);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while starting the quiz with ID {quizId}.", ex);
            }
        }

        public async Task<Quiz> EndQuizAsync(long teacherId, long quizId)
        {
            try
            {
                var quiz = await _quizRepository.Get(quizId);

                if (quiz.TeacherId != teacherId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to end this quiz.");
                }
                if (!quiz.IsActive)
                {
                    throw new InvalidOperationException("Quiz is not currently active.");
                }
                quiz.IsActive = false;

                var quizNotificationDTO = new QuizNotificationDTO
                {
                    Id = quiz.Id,
                    Title = quiz.Title,
                    Description = quiz.Description,
                    CreatedAt = DateTime.UtcNow,
                    DueDate = quiz.DueDate,
                    TeacherName = quiz.Teacher.FirstName + " " + quiz.Teacher.LastName,
                    SubjectName = quiz.Subject?.Name ?? "No Subject",
                    SubjectCode = quiz.Subject?.Code ?? "No Code",
                    ClassName = quiz.Classe?.Name ?? "No Class",
                    Type = "end"
                };

                string classGroupName = $"class_{quiz.ClassId}";
                await _hubContext.Clients.Group(classGroupName).SendAsync("NotifyEndQuiz", quizNotificationDTO);

                return await _quizRepository.Update(quizId, quiz);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }

            catch (Exception ex)
            {
                throw new Exception($"An error occurred while ending the quiz with ID {quizId}.", ex);
            }

        }


        public async Task<TeacherSummaryDTO> GetTeacherQuizSummaryAsync(long teacherId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {

                // Verify teacher exists
                var teacher = await _teacherRepository.Get(teacherId);


                var result = await _context.Set<TeacherSummaryDTO>()
                        .FromSqlRaw(
                            "select * from get_teacher_quiz_summary({0}, {1}::timestamp, {2}::timestamp)",
                            teacherId, startDate, endDate
                        )
                        .FirstOrDefaultAsync();


                if (result == null)
                {
                    throw new Exception($"Failed to retrieve quiz summary for teacher with ID {teacherId}.");
                }

                return result;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving quiz summary for teacher with ID {teacherId}.", ex);
            }
        }


        public async Task<IEnumerable<Teacher>> GetTeachersPaginationAsync(int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    throw new ArgumentException("Page number and page size must be greater than zero.");
                }

                var teachers = await _teacherRepository.GetAll();
                return teachers
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving teachers with pagination.", ex);
            }
        }

        public async Task<IEnumerable<Subject>> GetSubjectsByTeacherIdAsync(long teacherId)
        {
            try
            {
                var teacher = await _teacherRepository.Get(teacherId);
                var teacherSubjects = await _teacherSubjectRepository.GetAll();
                return teacherSubjects.Where(ts => ts.TeacherId == teacherId).Select(ts => ts.Subject).ToList();
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"No teacher found with ID {teacherId}.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving subjects for teacher with ID {teacherId}.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving subjects for teacher with ID {teacherId}.", ex);
            }
        }

        public async Task<IEnumerable<Classe>> GetClassesByTeacherIdAsync(long teacherId)
        {
            try
            {
                var teacher = await _teacherRepository.Get(teacherId);
                var teacherClasses = await _teacherClassRepository.GetAll();
                return teacherClasses.Where(tc => tc.TeacherId == teacherId).Select(tc => tc.Classe).ToList();
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"No teacher found with ID {teacherId}.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving classes for teacher with ID {teacherId}.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving classes for teacher with ID {teacherId}.", ex);
            }
        }
        public async Task<IEnumerable<Question>> GetQuestionsByTeacherIdAsync(long teacherId)
        {
            try
            {

                var teacher = await _teacherRepository.Get(teacherId);


                var result = await _context.Set<Question>()
                        .FromSqlRaw(
                            "select * from get_questions_by_teacher_id({0})",
                            teacherId
                        )
                        .ToListAsync();


                if (result == null)
                {
                    throw new Exception($"Failed to retrieve questions for teacher with ID {teacherId}.");
                }

                return result;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving questions for teacher with ID {teacherId}.", ex);
            }
        }

        private async Task<Teacher> AddSubjectsToTeacherAsync(long teacherId, ICollection<long> subjectIds)
        {
            try
            {
                var teacher = await _teacherRepository.Get(teacherId);

                foreach (var subjectId in subjectIds)
                {
                    var existingSubject = await _subjectRepository.Get(subjectId);

                    var teacherSubject = new TeacherSubject
                    {
                        TeacherId = teacherId,
                        SubjectId = subjectId
                    };

                    await _teacherSubjectRepository.Add(teacherSubject);
                }

                return teacher;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding subjects to the teacher.", ex);
            }
        }

        private async Task<Teacher> RemoveSubjectsFromTeacherAsync(long teacherId, ICollection<long> subjectIds)
        {
            try
            {
                var teacher = await _teacherRepository.Get(teacherId);

                foreach (var subjectId in subjectIds)
                {
                    var teacherSubject = await _teacherSubjectRepository.GetAll();
                    var existingTeacherSubject = teacherSubject.FirstOrDefault(ts => ts.TeacherId == teacherId && ts.SubjectId == subjectId);
                    if (existingTeacherSubject != null)
                    {
                        await _teacherSubjectRepository.Delete(existingTeacherSubject.Id);
                    }
                }

                return teacher;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing subjects from the teacher.", ex);
            }
        }

        private async Task<Teacher> AddClassesToTeacherAsync(long teacherId, ICollection<long> classIds)
        {
            try
            {
                var teacher = await _teacherRepository.Get(teacherId);

                foreach (var classId in classIds)
                {
                    var existingClass = await _classRepository.Get(classId);

                    var teacherClass = new TeacherClass
                    {
                        TeacherId = teacherId,
                        ClassId = classId
                    };

                    await _teacherClassRepository.Add(teacherClass);
                }

                return teacher;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding classes to the teacher.", ex);
            }
        }

        private async Task<Teacher> RemoveClassesFromTeacherAsync(long teacherId, ICollection<long> classIds)
        {
            try
            {
                var teacher = await _teacherRepository.Get(teacherId);

                foreach (var classId in classIds)
                {
                    var teacherClass = await _teacherClassRepository.GetAll();
                    var existingTeacherClass = teacherClass.FirstOrDefault(tc => tc.TeacherId == teacherId && tc.ClassId == classId);
                    if (existingTeacherClass != null)
                    {
                        await _teacherClassRepository.Delete(existingTeacherClass.Id);
                    }
                }

                return teacher;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing classes from the teacher.", ex);
            }
        }

    }
}