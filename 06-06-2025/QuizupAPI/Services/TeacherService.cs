using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Repositories;
using QuizupAPI.Models.DTOs.Teacher;
using QuizupAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using QuizupAPI.Misc.Mappers;
using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IRepository<long, Teacher> _teacherRepository;
        private readonly IRepository<long, Quiz> _quizRepository;
        private readonly IHubContext<QuizNotificationHub> _hubContext;
        public TeacherMapper teacherMapper;
        public TeacherService(IRepository<long, Teacher> teacherRepository, IRepository<long, Quiz> quizRepository, IHubContext<QuizNotificationHub> hubContext)
        {
            _quizRepository = quizRepository;
            _hubContext = hubContext;
            _teacherRepository = teacherRepository;
            teacherMapper = new TeacherMapper();
        }

        public async Task<Teacher> AddTeacherAsync(TeacherAddRequestDTO teacherDTO)
        {
            try
            {
                if (teacherDTO == null)
                {
                    throw new ArgumentNullException(nameof(teacherDTO), "Teacher data cannot be null.");
                }
                if (string.IsNullOrWhiteSpace(teacherDTO.FirstName) || string.IsNullOrWhiteSpace(teacherDTO.LastName) || string.IsNullOrWhiteSpace(teacherDTO.Email))
                {
                    throw new ArgumentException("First name, last name, and email are required fields.");
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
                if (existingTeacher == null)
                {
                    throw new KeyNotFoundException($"Teacher with ID {id} not found.");
                }

                var teacher = teacherMapper.MapTeacherUpdateRequestTeacher(teacherDTO);
                if (teacher == null)
                {
                    throw new Exception("Failed to map TeacherUpdateRequestDTO to Teacher.");
                }

                return await _teacherRepository.Update(id, teacher);
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
                return await _teacherRepository.Delete(id);
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
                if (quiz == null)
                {
                    throw new Exception($"Quiz with ID {quizId} not found.");
                }
                if (quiz.TeacherId != teacherId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to start this quiz.");
                }
                if (quiz.IsActive)
                {
                    throw new InvalidOperationException("Quiz is already active.");
                }
                quiz.IsActive = true;
                await _hubContext.Clients.All.SendAsync("NotifyStartQuiz", quiz);
                return await _quizRepository.Update(quizId, quiz);
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
                if (quiz == null)
                {
                    throw new Exception($"Quiz with ID {quizId} not found.");
                }
                if (quiz.TeacherId != teacherId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to end this quiz.");
                }
                if (!quiz.IsActive)
                {
                    throw new InvalidOperationException("Quiz is not currently active.");
                }
                quiz.IsActive = false;
                await _hubContext.Clients.All.SendAsync("NotifyEndQuiz", quiz);

                return await _quizRepository.Update(quizId, quiz);
            }

            catch (Exception ex)
            {
                throw new Exception($"An error occurred while ending the quiz with ID {quizId}.", ex);
            }
            
        }
    }
}