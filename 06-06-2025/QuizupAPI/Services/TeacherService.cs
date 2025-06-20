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
        private readonly QuizContext _context;

        public UserMapper userMapper;
        public TeacherMapper teacherMapper;
        public TeacherService(IRepository<long, Teacher> teacherRepository, IRepository<long, Quiz> quizRepository, IHubContext<QuizNotificationHub> hubContext, IEncryptionService encryptionService, IRepository<string, User> userRepository, QuizContext context)
        {
            _quizRepository = quizRepository;
            _hubContext = hubContext;
            _teacherRepository = teacherRepository;
            _encryptionService = encryptionService;
            _userRepository = userRepository;
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

                var allTeachers = await _teacherRepository.GetAll();
                var existingTeacher = allTeachers.FirstOrDefault(t => t.Email == teacherDTO.Email);
                if (existingTeacher != null)
                {
                    throw new InvalidOperationException("A teacher with this email already exists.");
                }

                var teacher = teacherMapper.MapTeacherAddRequestTeacher(teacherDTO);
                if (teacher == null)
                {
                    throw new Exception("Failed to map TeacherAddRequestDTO to Teacher.");
                }

                return await _teacherRepository.Add(teacher);
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

        public async Task<Teacher> UpdateTeacherAsync(long id, TeacherUpdateRequestDTO teacherDTO)
        {
            try
            {
                if (teacherDTO == null)
                {
                    throw new ArgumentNullException(nameof(teacherDTO), "Teacher data cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(teacherDTO.FirstName) || string.IsNullOrWhiteSpace(teacherDTO.LastName) || string.IsNullOrWhiteSpace(teacherDTO.Subject))
                {
                    throw new ArgumentException("First name, last name and subject are required fields.");
                }

                var existingTeacher = await _teacherRepository.Get(id);
                

                var teacher = teacherMapper.MapTeacherUpdateRequestTeacher(existingTeacher, teacherDTO);
                if (teacher == null)
                {
                    throw new Exception("Failed to map TeacherUpdateRequestDTO to Teacher.");
                }

                var updatedTeacher = await _teacherRepository.Update(id, teacher);
                if (updatedTeacher == null)
                {
                    throw new Exception($"Failed to update teacher with ID {id}.");
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
                    TeacherName = quiz.Teacher.FirstName + " " + quiz.Teacher.LastName
                };
                await _hubContext.Clients.All.SendAsync("NotifyStartQuiz", quizNotificationDTO);
           
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
                    TeacherName = quiz.Teacher.FirstName + " " + quiz.Teacher.LastName
                };

                await _hubContext.Clients.All.SendAsync("NotifyEndQuiz", quizNotificationDTO);

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
                

                // Execute the PostgreSQL function
                var result = await _context.Set<TeacherSummaryDTO>()
                    .FromSqlRaw("SELECT * FROM get_teacher_quiz_summary({0}, {1}, {2})",
                        teacherId,
                        startDate.HasValue ? startDate.Value : DBNull.Value,
                        endDate.HasValue ? endDate.Value : DBNull.Value)
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
    }
}