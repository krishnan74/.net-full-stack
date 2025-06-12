using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Repositories;
using QuizupAPI.Models.DTOs.Student;
using QuizupAPI.Models.DTOs.QuizSubmission;
using QuizupAPI.Models.DTOs.Answer;
using QuizupAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using QuizupAPI.Misc.Mappers;
using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<long, Student> _studentRepository;
        private readonly IRepository<long, Quiz> _quizRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<string, User> _userRepository;
        private readonly IRepository<long, QuizSubmission> _quizSubmissionRepository;
        private readonly IRepository<long, Answer> _answerRepository;

        public UserMapper userMapper;
        public StudentMapper studentMapper;
        public AnswerMapper answerMapper;

        public StudentService(IRepository<long, Student> studentRepository, IRepository<long, Quiz> quizRepository, IEncryptionService encryptionService, IRepository<string, User> userRepository, IRepository<long, QuizSubmission> quizSubmissionRepository, IRepository<long, Answer> answerRepository)
        {
            _quizRepository = quizRepository;
            _studentRepository = studentRepository;
            _encryptionService = encryptionService;
            _userRepository = userRepository;
            _quizSubmissionRepository = quizSubmissionRepository;
            _answerRepository = answerRepository;
            studentMapper = new StudentMapper();
            userMapper = new UserMapper();
            answerMapper = new AnswerMapper();
        }

        public async Task<Student> AddStudentAsync(StudentAddRequestDTO studentDTO)
        {
            try
            {


                if (studentDTO == null)
                {
                    throw new ArgumentNullException(nameof(studentDTO), "Student data cannot be null.");
                }
                if (string.IsNullOrWhiteSpace(studentDTO.FirstName) || string.IsNullOrWhiteSpace(studentDTO.LastName) || string.IsNullOrWhiteSpace(studentDTO.Email) || string.IsNullOrWhiteSpace(studentDTO.Password))
                {
                    throw new ArgumentException("First name, last name, email, and password are required fields.");
                }
                if (!new EmailAddressAttribute().IsValid(studentDTO.Email))
                {
                    throw new ArgumentException("Invalid email format.");
                }

                var user = userMapper.MapStudentAddRequestUser(studentDTO);

                if (user == null)
                {
                    throw new Exception("Failed to map StudentAddRequestDTO to User.");
                }

                var hashedPassword = _encryptionService.HashPassword(studentDTO.Password);

                user.HashedPassword = hashedPassword;

                user = await _userRepository.Add(user);

                if (user == null)
                {
                    throw new Exception("Failed to add user.");
                }

                var allStudents = await _studentRepository.GetAll();
                var existingStudent = allStudents.FirstOrDefault(t => t.Email == studentDTO.Email);
                if (existingStudent != null)
                {
                    throw new InvalidOperationException("A student with this email already exists.");
                }

                var student = studentMapper.MapStudentAddRequestStudent(studentDTO);
                if (student == null)
                {
                    throw new Exception("Failed to map StudentAddRequestDTO to Student.");
                }

                return await _studentRepository.Add(student);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the student.", ex);
            }
        }

        public async Task<Student> GetStudentByIdAsync(long id)
        {
            try
            {
                return await _studentRepository.Get(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the student with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            try
            {
                return await _studentRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all students.", ex);
            }
        }

        public async Task<Student> UpdateStudentAsync(long id, StudentUpdateRequestDTO studentDTO)
        {
            try
            {
                if (studentDTO == null)
                {
                    throw new ArgumentNullException(nameof(studentDTO), "Student data cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(studentDTO.FirstName) || string.IsNullOrWhiteSpace(studentDTO.LastName) || string.IsNullOrWhiteSpace(studentDTO.Class))
                {
                    throw new ArgumentException("First name, last name and class are required fields.");
                }

                var existingStudent = await _studentRepository.Get(id);
                if (existingStudent == null)
                {
                    throw new KeyNotFoundException($"Student with ID {id} not found.");
                }

                var student = studentMapper.MapStudentUpdateRequestStudent(studentDTO);
                if (student == null)
                {
                    throw new Exception("Failed to map StudentUpdateRequestDTO to Student.");
                }

                return await _studentRepository.Update(id, student);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the student with ID {id}.", ex);
            }
        }

        public async Task<Student> DeleteStudentAsync(long id)
        {
            try
            {
                return await _studentRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the student with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<QuizSubmission>> GetSubmissionsByStudentIdAsync(long id)
        {
            try
            {
                var quizSubmission = await _quizSubmissionRepository.GetAll();
                if (quizSubmission == null || !quizSubmission.Any())
                {
                    throw new KeyNotFoundException($"No quiz submissions found for student with ID {id}.");
                }
                quizSubmission = quizSubmission.Where(qs => qs.StudentId == id).ToList();
                if (quizSubmission == null || !quizSubmission.Any())
                {
                    throw new KeyNotFoundException($"No quiz submissions found for student with ID {id}.");
                }
                return quizSubmission;

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving quiz submissions for student with ID {id}.", ex);
            }
        }

        public async Task<QuizSubmission> StartQuizAsync(long studentId, long quizId)
        {
            try
            {
                var student = await _studentRepository.Get(studentId);
                if (student == null)
                {
                    throw new KeyNotFoundException($"Student with ID {studentId} not found.");
                }

                var quiz = await _quizRepository.Get(quizId);
                if (quiz == null)
                {
                    throw new KeyNotFoundException($"Quiz with ID {quizId} not found.");
                }

                var quizSubmission = new QuizSubmission();
                quizSubmission.StudentId = studentId;
                quizSubmission.QuizId = quizId;
                quizSubmission.SubmissionStatus = "InProgress";

                return await _quizSubmissionRepository.Add(quizSubmission);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while starting the quiz.", ex);
            }
        }

        public async Task<QuizSubmission> SubmitQuizAsync(long studentId, long quizSubmissionId, QuizSubmissionDTO submission)
        {
            try
            {
                if (submission == null)
                {
                    throw new ArgumentNullException(nameof(submission), "Submission data cannot be null.");
                }

                var existingQuizSubmission = await _quizSubmissionRepository.Get(quizSubmissionId);

                if (existingQuizSubmission == null)
                {
                    throw new KeyNotFoundException($"QuizSubmission with ID {quizSubmissionId} not found.");
                }

                var student = await _studentRepository.Get(studentId);
                if (student == null)
                {
                    throw new KeyNotFoundException($"Student with ID {studentId} not found.");
                }

                if( existingQuizSubmission.StudentId != studentId )
                {
                    throw new UnauthorizedAccessException("You are not authorized to submit this quiz.");
                }

                existingQuizSubmission.SubmissionStatus = "Submitted";
                existingQuizSubmission.SubmissionDate = DateTime.Now;

                await MapAndAddAnswersAsync(existingQuizSubmission.Answers, submission.Answers);

                int score = await GetScoreAsync(submission);
                existingQuizSubmission.Score = score;

                var updatedQuizSubmission = await _quizSubmissionRepository.Update(quizSubmissionId, existingQuizSubmission);

                if (updatedQuizSubmission == null)
                {
                    throw new Exception("Failed to update quiz submission.");
                }

                return updatedQuizSubmission;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while submitting the quiz.", ex);
            }
        }
        public async Task<QuizSubmission> SaveAnswersAsync(long studentId, long quizSubmissionId, QuizSubmissionDTO submission)
        {
             try
            {
                if (submission == null)
                {
                    throw new ArgumentNullException(nameof(submission), "Submission data cannot be null.");
                }

                var existingQuizSubmission = await _quizSubmissionRepository.Get(quizSubmissionId);
                if (existingQuizSubmission == null)
                {
                    throw new KeyNotFoundException($"QuizSubmission with ID {quizSubmissionId} not found.");
                }

                var student = await _studentRepository.Get(studentId);
                if (student == null)
                {
                    throw new KeyNotFoundException($"Student with ID {studentId} not found.");
                }

                if (existingQuizSubmission.StudentId != studentId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to save answers for this quiz.");
                }

                existingQuizSubmission.SubmissionStatus = "InProgress";
                existingQuizSubmission.SavedDate = DateTime.Now;

                await MapAndAddAnswersAsync(existingQuizSubmission.Answers, submission.Answers);

                var updatedQuizSubmission = await _quizSubmissionRepository.Update(quizSubmissionId, existingQuizSubmission);

                if (updatedQuizSubmission == null)
                {
                    throw new Exception("Failed to update quiz submission.");
                }

                return updatedQuizSubmission;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while submitting the quiz.", ex);
            }
            
        }

        private async Task MapAndAddAnswersAsync(List<Answer> existingAnswers, List<AnswerAddRequestDTO> currentAnswers)
        {
            if (currentAnswers == null || currentAnswers.Count == 0)
            {
            throw new ValidationException("At least one answer is required.");
            }

            foreach (var answerDTO in currentAnswers)
            {
                // Check if an answer for the same question already exists
                var existingAnswer = existingAnswers.FirstOrDefault(a => a.QuestionId == answerDTO.QuestionId);

                // Add the answer if it doesn't exist
                if (existingAnswer == null)
                {
                    var answer = answerMapper.MapAnswerDTOToAnswer(answerDTO);
                    if (answer == null)
                    {
                        throw new Exception("Failed to map answer from DTO.");
                    }
                    var addedAnswer = await _answerRepository.Add(answer);

                    if (addedAnswer == null)
                    {
                        throw new Exception("Failed to add answer");
                    }

                    continue;
                }

                // Update the existing answer if it exists
                if (!string.Equals(existingAnswer.SelectedAnswer, answerDTO.SelectedAnswer, StringComparison.OrdinalIgnoreCase))
                {
                    existingAnswer.SelectedAnswer = answerDTO.SelectedAnswer;
                    var updatedAnswer = await _answerRepository.Update(existingAnswer.Id, existingAnswer);
                    if (updatedAnswer == null)
                    {
                        throw new Exception("Failed to update answer");
                    }
                }
                
            }
        }

        private async Task<int> GetScoreAsync(QuizSubmissionDTO submission)
        {
            var quiz = await _quizRepository.Get(submission.QuizId);

            int correctAnswers = 0;

            // Get all questions for this quiz via the QuizQuestions navigation property
            var quizQuestions = quiz.QuizQuestions;
            
            if (quizQuestions == null || !quizQuestions.Any())
            {
                throw new Exception("No questions found for this quiz.");
            }

            var questions = quizQuestions.Select(qq => qq.Question).ToList();

            foreach (var answerDTO in submission.Answers)
            {
                var question = questions.FirstOrDefault(q => q.Id == answerDTO.QuestionId);
                if (question == null)
                {
                    continue;
                }

                bool isCorrect = question.CorrectAnswer.Equals(answerDTO.SelectedAnswer, StringComparison.OrdinalIgnoreCase);
                if (isCorrect) correctAnswers++;
            }

            var totalQuestions = questions.Count;
            int score = (int)((double)correctAnswers / totalQuestions * 100);

            return score;
        }
    }
    
}