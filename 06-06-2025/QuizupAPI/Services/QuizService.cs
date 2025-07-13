using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Repositories;
using QuizupAPI.Models.DTOs.Quiz;
using QuizupAPI.Models.DTOs.Question;
using QuizupAPI.Models.SearchModels;
using QuizupAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using QuizupAPI.Misc.Mappers;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Services
{
    public class QuizService : IQuizService
    {
        private readonly IRepository<long, Quiz> _quizRepository;
        private readonly IRepository<long, Question> _questionRepository;
        private readonly IRepository<long, QuizQuestion> _quizQuestionRepository;
        private readonly IRepository<long, QuizSubmission> _quizSubmissionRepository;
        private readonly IRepository<long, Teacher> _teacherRepository;
        private readonly IRepository<long, Student> _studentRepository;
        private readonly IRepository<long, Classe> _classRepository;
        public QuizMapper quizMapper;
        public QuestionMapper questionMapper;

        public QuizService(IRepository<long, Quiz> quizRepository,
                            IRepository<long, Question> questionRepository,
                            IRepository<long, QuizQuestion> quizQuestionRepository,
                            IRepository<long, QuizSubmission> quizSubmissionRepository,
                            IRepository<long, Teacher> teacherRepository, 
                            IRepository<long, Student> studentRepository, 
                            IRepository<long, Classe> classRepository
                        )
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _quizQuestionRepository = quizQuestionRepository;
            _quizSubmissionRepository = quizSubmissionRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _classRepository = classRepository;
            quizMapper = new QuizMapper();
            questionMapper = new QuestionMapper();
        }
        public async Task<Quiz> AddQuizAsync(QuizAddRequestDTO quizAddRequestDTO)
        {
            if (quizAddRequestDTO == null)
                throw new ArgumentNullException(nameof(quizAddRequestDTO), "Quiz cannot be null");

            if (string.IsNullOrWhiteSpace(quizAddRequestDTO.Title) ||
                string.IsNullOrWhiteSpace(quizAddRequestDTO.Description))
                throw new ValidationException("Title and description are required.");

            try
            {
                var quiz = quizMapper.MapQuizAddRequestQuiz(quizAddRequestDTO);
                var addedQuiz = await _quizRepository.Add(quiz);

                if (addedQuiz == null)
                    throw new Exception("Failed to add quiz");

                await MapAndAddQuestionsAsync(addedQuiz.Id, quizAddRequestDTO);

                return addedQuiz;
            }
            catch (ValidationException) { throw; }
            catch (ArgumentNullException) { throw; }
            catch (ArgumentException) { throw; }
            catch (KeyNotFoundException) { throw; }
            catch (UnauthorizedAccessException) { throw; }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the quiz.", ex);
            }
        }

        public async Task<Quiz> GetQuizByIdAsync(long id)
        {
            try
            {
                var quiz = await _quizRepository.Get(id);
                
                return quiz;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the quiz: {ex.Message}");
            }
        }
        public async Task<IEnumerable<Quiz>> GetAllQuizzesAsync()
        {
            try
            {
                var quizzes = await _quizRepository.GetAll();
                return quizzes;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving all quizzes: {ex.Message}");
            }
        }
        public async Task<Quiz> UpdateQuizAsync(long quizId, long teacherId, QuizUpdateRequestDTO quizUpdateRequestDTO)
        {
            try
            {
                if (quizUpdateRequestDTO == null)
                {
                    throw new ArgumentNullException(nameof(quizUpdateRequestDTO), "Quiz data cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(quizUpdateRequestDTO.Title) || string.IsNullOrWhiteSpace(quizUpdateRequestDTO.Description))
                {
                    throw new ArgumentException("Title and description are required fields.");
                }

                var existingQuiz = await _quizRepository.Get(quizId);
                

                if (existingQuiz.TeacherId != teacherId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to update this quiz.");
                }

                var quiz = quizMapper.MapQuizUpdateRequestQuiz(existingQuiz, quizUpdateRequestDTO);
                if (quiz == null)
                {
                    throw new Exception("Failed to map quiz from DTO.");
                }

                var updatedQuiz = await _quizRepository.Update(quizId, quiz);

                if (updatedQuiz == null)
                {
                    throw new Exception("Failed to update quiz");
                }

                return updatedQuiz;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(ex.ParamName, ex.Message);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the quiz: {ex.Message}");
            }
        }
        public async Task<Quiz> DeleteQuizAsync(long id, long teacherId)
        {
            try
            {
                var quiz = await _quizRepository.Get(id);
                
                if (quiz.TeacherId != teacherId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to delete this quiz.");
                }
                return await _quizRepository.Delete(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the quiz: {ex.Message}");
            }
        }
        public async Task<Question> AddQuestionToQuizAsync(long quizId, long teacherId, QuestionAddRequestDTO questionAddRequestDTO)
        {
            if (questionAddRequestDTO == null)
            {
                throw new ArgumentNullException(nameof(questionAddRequestDTO), "Question cannot be null");
            }

            if (string.IsNullOrWhiteSpace(questionAddRequestDTO.Text) || questionAddRequestDTO.Options == null || questionAddRequestDTO.Options.Count == 0)
            {
                throw new ValidationException("Question text and options are required.");
            }

            var question = questionMapper.MapQuestionAddRequestQuestion(questionAddRequestDTO);
            if (question == null)
            {
                throw new Exception("Failed to map question from DTO.");
            }

            var addedQuestion = await _questionRepository.Add(question);
            if (addedQuestion == null)
            {
                throw new Exception("Failed to add question");
            }

            var quizQuestion = new QuizQuestion
            {
                QuizId = quizId,
                QuestionId = addedQuestion.Id
            };
            var addedQuizQuestion = await _quizQuestionRepository.Add(quizQuestion);
            if (addedQuizQuestion == null)
            {
                throw new Exception("Failed to add question to quiz");
            }

            return addedQuestion;
        }
        public async Task<Question> UpdateQuestionAsync(long questionId, long teacherId, QuestionUpdateRequestDTO questionUpdateRequestDTO)
        {
            if (questionUpdateRequestDTO == null)
            {
                throw new ArgumentNullException(nameof(questionUpdateRequestDTO), "Question cannot be null");
            }

            if (string.IsNullOrWhiteSpace(questionUpdateRequestDTO.Text) || questionUpdateRequestDTO.Options == null || questionUpdateRequestDTO.Options.Count == 0 || string.IsNullOrWhiteSpace(questionUpdateRequestDTO.CorrectAnswer))
            {
                throw new ValidationException("Question text, options, and correct answer are required.");
            }

            var existingQuestion = await _questionRepository.Get(questionId);

            var question = questionMapper.MapQuestionUpdateRequestQuestion(existingQuestion, questionUpdateRequestDTO);
            if (question == null)
            {
                throw new Exception("Failed to map question from DTO.");
            }

            var updatedQuestion = await _questionRepository.Update(questionId, question);
            if (updatedQuestion == null)
            {
                throw new Exception("Failed to update question");
            }

            return updatedQuestion;
        }

        public async Task<IEnumerable<Quiz>> GetQuizzesPaginationAsync(int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    throw new ArgumentException("Page number and size must be greater than zero.");
                }

                var quizzes = await _quizRepository.GetAll();
                return quizzes
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving quizzes: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Quiz>> SearchQuiz(QuizSearchModel quizSearchModel)
        {
            try
            {
                IEnumerable<Quiz> quizzes;

                if (quizSearchModel.Role == "Teacher" && quizSearchModel.SearchId != null)
                {
                    var teacher = await _teacherRepository.Get((long)quizSearchModel.SearchId);
                    quizzes = teacher.Quizzes.AsEnumerable();
                }
                else if (quizSearchModel.Role == "Student" && quizSearchModel.SearchId != null)
                {
                    var student = await _studentRepository.Get((long)quizSearchModel.SearchId);
                    var classe = await _classRepository.Get(student.ClassId);
                    quizzes = classe.Quizzes.AsEnumerable();

                }
                else
                {
                    quizzes = await _quizRepository.GetAll();
                    Console.WriteLine("Searching all quizzes");
                }

                // Apply OR logic for Title, Description, and TeacherName
                if (!string.IsNullOrWhiteSpace(quizSearchModel.Title) ||
                    !string.IsNullOrWhiteSpace(quizSearchModel.Description) ||
                    !string.IsNullOrWhiteSpace(quizSearchModel.TeacherName) || 
                    !string.IsNullOrWhiteSpace(quizSearchModel.Tags)
                    )
                {
                    quizzes = quizzes.Where(q =>
                        (!string.IsNullOrWhiteSpace(quizSearchModel.Title) && q.Title.ToLower().Contains(quizSearchModel.Title.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(quizSearchModel.Description) && q.Description.ToLower().Contains(quizSearchModel.Description.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(quizSearchModel.TeacherName) &&
                            (q.Teacher.FirstName.ToLower().Contains(quizSearchModel.TeacherName.ToLower()) ||
                             q.Teacher.LastName.ToLower().Contains(quizSearchModel.TeacherName.ToLower()))
                        ) ||
                        (!string.IsNullOrWhiteSpace(quizSearchModel.Tags) &&
                            q.Tags != null &&
                            q.Tags.Any(tag => tag != null && tag.ToLower().Contains(quizSearchModel.Tags.ToLower()))
                        )
                    );
                }

                // Apply CreatedAt filters
                if (quizSearchModel.CreatedAtMin != null)
                {
                    Console.WriteLine($"Filtering quizzes created after: {quizSearchModel.CreatedAtMin}");
                    quizzes = quizzes.Where(q => q.CreatedAt >= quizSearchModel.CreatedAtMin);
                }
                if (quizSearchModel.CreatedAtMax != null)
                {
                    Console.WriteLine($"Filtering quizzes created before: {quizSearchModel.CreatedAtMax}");
                    quizzes = quizzes.Where(q => q.CreatedAt <= quizSearchModel.CreatedAtMax);
                }

                // Apply DueDate filters
                if (quizSearchModel.DueDateMin != null)
                {
                    Console.WriteLine($"Filtering quizzes due after: {quizSearchModel.DueDateMin}");
                    quizzes = quizzes.Where(q => q.DueDate >= quizSearchModel.DueDateMin);
                }
                if (quizSearchModel.DueDateMax != null)
                {
                    Console.WriteLine($"Filtering quizzes due before: {quizSearchModel.DueDateMax}");
                    quizzes = quizzes.Where(q => q.DueDate <= quizSearchModel.DueDateMax);
                }

                // Apply ClassId filter
                if (quizSearchModel.ClassId != null)
                {
                    Console.WriteLine($"Filtering quizzes for ClassId: {quizSearchModel.ClassId}");
                    quizzes = SearchByClassId(quizzes, quizSearchModel.ClassId);
                }

                // Apply SubjectId filter
                if (quizSearchModel.SubjectId != null)
                {
                    Console.WriteLine($"Filtering quizzes for SubjectId: {quizSearchModel.SubjectId}");
                    quizzes = SearchBySubjectId(quizzes, quizSearchModel.SubjectId);
                }

                quizzes = quizzes.Where(q => q.IsActive == quizSearchModel.IsActive);

                return quizzes.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to search quizzes", ex);
            }
        }

        public async Task<QuizSubmission> GetQuizSubmissionByIdAsync(long quizSubmissionId)
        {
            try
            {
                var quizSubmission = await _quizSubmissionRepository.Get(quizSubmissionId);
                return quizSubmission;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Quiz submission with ID {quizSubmissionId} not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the quiz submission: {ex.Message}", ex);
            }
        }
        private IEnumerable<Quiz> SearchByClassId(
            IEnumerable<Quiz> quizzes, long? classId
        )
        {
            if (classId == null)
            {
                return quizzes;
            }

            return quizzes.Where(q => q.ClassId == classId).ToList();
        }

        private IEnumerable<Quiz> SearchBySubjectId(
            IEnumerable<Quiz> quizzes, long? subjectId
        )
        {
            if (subjectId == null)
            {
                return quizzes;
            }

            return quizzes.Where(q => q.SubjectId == subjectId).ToList();
        }

        private async Task MapAndAddQuestionsAsync(long quizId, QuizAddRequestDTO quizAddRequestDTO)
        {
            if (quizAddRequestDTO.Questions == null || quizAddRequestDTO.Questions.Count == 0)
            {
                throw new ValidationException("At least one question is required.");
            }

            foreach (var questionDTO in quizAddRequestDTO.Questions)
            {
                long? questionId = questionDTO.Id;

                if (questionId == null)
                {

                    var question = questionMapper.MapQuestionAddRequestQuestion(questionDTO);

                    if (question == null)
                    {
                        throw new Exception("Failed to map question from DTO.");
                    }

                    var addedQuestion = await _questionRepository.Add(question);

                    if (addedQuestion == null)
                    {
                        throw new Exception("Failed to add question");
                    }

                    questionId = addedQuestion.Id;
                }

                var quizQuestion = new QuizQuestion
                {
                    QuizId = quizId,
                    QuestionId = questionId
                };

                var addedQuizQuestion = await _quizQuestionRepository.Add(quizQuestion);
                if (addedQuizQuestion == null)
                {
                    throw new Exception("Failed to add question to quiz");
                }

            }
        }
        

        
   }
}