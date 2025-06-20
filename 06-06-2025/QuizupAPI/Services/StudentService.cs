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
using QuizupAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
        private readonly QuizContext _context;

        public UserMapper userMapper;
        public StudentMapper studentMapper;
        public AnswerMapper answerMapper;

        public StudentService(IRepository<long, Student> studentRepository, IRepository<long, Quiz> quizRepository, IEncryptionService encryptionService, IRepository<string, User> userRepository, IRepository<long, QuizSubmission> quizSubmissionRepository, IRepository<long, Answer> answerRepository, QuizContext context)
        {
            _quizRepository = quizRepository;
            _studentRepository = studentRepository;
            _encryptionService = encryptionService;
            _userRepository = userRepository;
            _quizSubmissionRepository = quizSubmissionRepository;
            _answerRepository = answerRepository;
            _context = context;
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
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
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

                var student = studentMapper.MapStudentUpdateRequestStudent(existingStudent, studentDTO);
                if (student == null)
                {
                    throw new Exception("Failed to map StudentUpdateRequestDTO to Student.");
                }

                var updatedStudent = await _studentRepository.Update(id, student);
                if (updatedStudent == null)
                {
                    throw new Exception($"Failed to update student with ID {id}.");
                }

                return updatedStudent;
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
                var existingStudent = await _studentRepository.Get(id);

                await _userRepository.Delete(existingStudent.Email);
                
                return await _studentRepository.Delete(id);
            }

            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
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
                var student = await _studentRepository.Get(id);
                
                var quizSubmissions = await _quizSubmissionRepository.GetAll();
                if (quizSubmissions == null || !quizSubmissions.Any())
                {
                    return Enumerable.Empty<QuizSubmission>();
                }
                quizSubmissions = quizSubmissions.Where(qs => qs.StudentId == id).ToList();
                if (quizSubmissions == null || !quizSubmissions.Any())
                {
                    return Enumerable.Empty<QuizSubmission>();
                }

                return quizSubmissions;

            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
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
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
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
                Console.WriteLine("Starting SubmitQuizAsync...");

                Console.WriteLine($"Fetching quiz submission with ID {quizSubmissionId}...");
                var existingQuizSubmission = await _quizSubmissionRepository.Get(quizSubmissionId);

                Console.WriteLine($"Fetching student with ID {studentId}...");
                var student = await _studentRepository.Get(studentId);

                if (existingQuizSubmission.StudentId != studentId)
                {
                    Console.WriteLine("Unauthorized access: student does not match submission.");
                    throw new UnauthorizedAccessException("You are not authorized to submit this quiz.");
                }

                Console.WriteLine("Setting submission status to Submitted and updating SubmissionDate...");
                existingQuizSubmission.SubmissionStatus = "Submitted";
                existingQuizSubmission.SubmissionDate = DateTime.UtcNow;

                if (submission.Answers.Count != 0)
                {
                    Console.WriteLine("Mapping and adding/updating answers...");
                    await MapAndAddAnswersAsync(existingQuizSubmission.Answers, submission.Answers, quizSubmissionId);
                }

                Console.WriteLine("Calculating score...");
                int score = await GetScoreAsync(existingQuizSubmission);
                existingQuizSubmission.Score = score;

                Console.WriteLine("Updating quiz submission in repository...");
                var updatedQuizSubmission = await _quizSubmissionRepository.Update(quizSubmissionId, existingQuizSubmission);

                if (updatedQuizSubmission == null)
                {
                    Console.WriteLine("Failed to update quiz submission.");
                    throw new Exception("Failed to update quiz submission.");
                }

                Console.WriteLine("SubmitQuizAsync completed successfully.");
                return updatedQuizSubmission;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
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
                Console.WriteLine("Starting SaveAnswersAsync...");

                if (submission == null)
                {
                    Console.WriteLine("Submission data is null.");
                    throw new ArgumentNullException(nameof(submission), "Submission data cannot be null.");
                }

                Console.WriteLine($"Fetching quiz submission with ID {quizSubmissionId}...");
                var existingQuizSubmission = await _quizSubmissionRepository.Get(quizSubmissionId);

                Console.WriteLine($"Fetching student with ID {studentId}...");
                var student = await _studentRepository.Get(studentId);

                if (existingQuizSubmission.StudentId != studentId)
                {
                    Console.WriteLine("Unauthorized access: student does not match submission.");
                    throw new UnauthorizedAccessException("You are not authorized to save answers for this quiz.");
                }

                Console.WriteLine("Setting submission status to InProgress and updating SavedDate...");
                existingQuizSubmission.SubmissionStatus = "Saved";
                existingQuizSubmission.SavedDate = DateTime.UtcNow;

                Console.WriteLine("Mapping and adding/updating answers...");
                await MapAndAddAnswersAsync(existingQuizSubmission.Answers, submission.Answers, quizSubmissionId);

                Console.WriteLine("Updating quiz submission in repository...");
                var updatedQuizSubmission = await _quizSubmissionRepository.Update(quizSubmissionId, existingQuizSubmission);

                if (updatedQuizSubmission == null)
                {
                    Console.WriteLine("Failed to update quiz submission.");
                    throw new Exception("Failed to update quiz submission.");
                }

                Console.WriteLine("SaveAnswersAsync completed successfully.");
                return updatedQuizSubmission;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private async Task MapAndAddAnswersAsync(List<Answer> existingAnswers, List<AnswerAddRequestDTO> currentAnswers, long quizSubmissionId )
        {
            Console.WriteLine("Starting MapAndAddAnswersAsync...");

            if (currentAnswers == null || currentAnswers.Count == 0)
            {
            Console.WriteLine("No answers provided in currentAnswers.");
            throw new ValidationException("At least one answer is required.");
            }

            foreach (var answerDTO in currentAnswers)
            {
            Console.WriteLine($"Processing answer for QuestionId: {answerDTO.QuestionId}");

            Answer existingAnswer = null;

            if (existingAnswers != null)
            {
                Console.WriteLine("Checking for existing answers...");
                existingAnswer = existingAnswers.FirstOrDefault(a => a.QuestionId == answerDTO.QuestionId);
                Console.WriteLine(existingAnswer != null ? "Existing answer found." : "No existing answer found.");
            }

            // Check if an answer for the same question already exists

            // Add the answer if it doesn't exist
            if (existingAnswer == null)
            {
                Console.WriteLine("No existing answer found. Mapping and adding new answer...");
                var answer = answerMapper.MapAnswerDTOToAnswer(answerDTO, quizSubmissionId);
                if (answer == null)
                {
                Console.WriteLine("Failed to map answer from DTO.");
                throw new Exception("Failed to map answer from DTO.");
                }
                var addedAnswer = await _answerRepository.Add(answer);

                if (addedAnswer == null)
                {
                Console.WriteLine("Failed to add answer.");
                throw new Exception("Failed to add answer");
                }

                Console.WriteLine("Answer added successfully.");
                continue;
            }

            // Update the existing answer if it exists
            if (!string.Equals(existingAnswer.SelectedAnswer, answerDTO.SelectedAnswer, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Existing answer found but selected answer is different. Updating answer...");
                existingAnswer.SelectedAnswer = answerDTO.SelectedAnswer;
                var updatedAnswer = await _answerRepository.Update(existingAnswer.Id, existingAnswer);
                if (updatedAnswer == null)
                {
                Console.WriteLine("Failed to update answer.");
                throw new Exception("Failed to update answer");
                }
                Console.WriteLine("Answer updated successfully.");
            }
            else
            {
                Console.WriteLine("Existing answer found and selected answer is the same. No update needed.");
            }
            }

        }

        private async Task<int> GetScoreAsync(QuizSubmission submission)
        {
            Console.WriteLine("Fetching quiz for submission...");
            var quiz = await _quizRepository.Get(submission.QuizId);

            Console.WriteLine("Initializing correctAnswers counter...");
            int correctAnswers = 0;

            Console.WriteLine("Getting quiz questions...");
            var quizQuestions = quiz.QuizQuestions;

            if (quizQuestions == null || !quizQuestions.Any())
            {
                Console.WriteLine("No questions found for this quiz.");
                throw new Exception("No questions found for this quiz.");
            }

            var questions = quizQuestions.Select(qq => qq.Question).ToList();

            Console.WriteLine("Iterating through submitted answers...");
            foreach (var answerDTO in submission.Answers)
            {
                Console.WriteLine($"Checking answer for QuestionId: {answerDTO.QuestionId}");
                var question = questions.FirstOrDefault(q => q.Id == answerDTO.QuestionId);
                if (question == null)
                {
                    Console.WriteLine("Question not found, skipping...");
                    continue;
                }

                Console.WriteLine($"Selected answer: {answerDTO.SelectedAnswer}");
                Console.WriteLine($"Correct answer: {question.CorrectAnswer}");

                bool isCorrect = question.CorrectAnswer.Equals(answerDTO.SelectedAnswer, StringComparison.OrdinalIgnoreCase);
                Console.WriteLine(isCorrect ? "Answer is correct." : "Answer is incorrect.");
                if (isCorrect) correctAnswers++;
            }

            var totalQuestions = questions.Count;
            Console.WriteLine($"Total correct answers: {correctAnswers} out of {totalQuestions}");

            int score = (int)((double)correctAnswers / totalQuestions * 100);

            Console.WriteLine($"Calculated score: {score}");

            return score;
        }

        public async Task<StudentSummaryDTO> GetStudentQuizSummaryAsync(long studentId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Verify student exists
                var student = await _studentRepository.Get(studentId);
                

                // Execute the PostgreSQL function
                var result = await _context.Set<StudentSummaryDTO>()
                    .FromSqlRaw("SELECT * FROM get_student_quiz_summary({0}, {1}, {2})",
                        studentId,
                        startDate.HasValue ? startDate.Value : DBNull.Value,
                        endDate.HasValue ? endDate.Value : DBNull.Value)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    throw new Exception($"Failed to retrieve quiz summary for student with ID {studentId}.");
                }

                return result;
            }
             catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving quiz summary for student with ID {studentId}.", ex);
            }
        }
        
        public async Task<IEnumerable<Student>> GetStudentsPaginationAsync(int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    throw new ArgumentException("Page number and page size must be greater than zero.");
                }

                var students = await _studentRepository.GetAll();
                return students
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving paginated students.", ex);
            }
        }
    }
    
}