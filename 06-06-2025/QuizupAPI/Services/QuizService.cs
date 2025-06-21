using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Repositories;
using QuizupAPI.Models.DTOs.Quiz;
using QuizupAPI.Models.DTOs.Question;
using QuizupAPI.Models.SearchModels;
using QuizupAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using QuizupAPI.Misc.Mappers;
using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Services
{
    public class QuizService : IQuizService
    {
        private readonly IRepository<long, Quiz> _quizRepository;
        private readonly IRepository<long, Question> _questionRepository;
        private readonly IRepository<long, QuizQuestion> _quizQuestionRepository;
        public QuizMapper quizMapper;
        public QuestionMapper questionMapper;

        public QuizService(IRepository<long, Quiz> quizRepository,
                            IRepository<long, Question> questionRepository,
                            IRepository<long, QuizQuestion> quizQuestionRepository)
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _quizQuestionRepository = quizQuestionRepository;
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
                var quizzes = await _quizRepository.GetAll();
                quizzes = SearchByTitle(quizzes, quizSearchModel.Title);

                Console.WriteLine($"Searching by title: {quizSearchModel.Title}");
                Console.WriteLine($"Number of quizzes after title search: {quizzes.Count()}");  
                // quizzes = SearchByTeacherName(quizzes, quizSearchModel.TeacherName);
                // quizzes = SearchByCreatedAt(quizzes, quizSearchModel.CreatedAt);
                // quizzes = SearchByDueDate(quizzes, quizSearchModel.DueDate);
                // quizzes = SearchByIsActive(quizzes, quizSearchModel.IsActive);

                return quizzes.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to search quiz", ex);
            }
        }

        private IEnumerable<Quiz> SearchByTitle(
            IEnumerable<Quiz> quizzes, string title
        ) {
            if (title == "" || title == null)
            {
                Console.WriteLine("No title provided, returning all quizzes.");
                return quizzes;
            }
            else
            {
                Console.WriteLine($"Searching quizzes with title containing: {title}");
                var filteredQuizzes = quizzes.Where(q => q.Title.ToLower().Contains(title.ToLower())).ToList();
            }
        }

        private IEnumerable<Quiz> SearchByTeacherName(
            IEnumerable<Quiz> quizzes, string teacherName
        )
        {
            if (teacherName == "" || teacherName == null)
            {
                return quizzes;
            }
            else
            {
                return quizzes.Where(q => q.Teacher.FirstName.ToLower().Contains(teacherName.ToLower()
                ) || q.Teacher.LastName.ToLower().Contains(teacherName.ToLower()
                
                )).ToList();
            }
        }

        private IEnumerable<Quiz> SearchByCreatedAt(
            IEnumerable<Quiz> quizzes, Range<DateTime>? createdAt
        )
        {
            if (createdAt == null || (createdAt.Min == null && createdAt.Max == null))
            {
                return quizzes;
            }

            return quizzes.Where(q =>
                (createdAt.Min == null || q.CreatedAt >= createdAt.Min) &&
                (createdAt.Max == null || q.CreatedAt <= createdAt.Max)
            ).ToList();
        }

        private IEnumerable<Quiz> SearchByDueDate(
            IEnumerable<Quiz> quizzes, Range<DateTime>? dueDate
        )
        {
            if (dueDate == null || (dueDate.Min == null && dueDate.Max == null))
            {
                return quizzes;
            }

            return quizzes.Where(q =>
                (dueDate.Min == null || q.DueDate >= dueDate.Min) &&
                (dueDate.Max == null || q.DueDate <= dueDate.Max)
            ).ToList();
        }

        private IEnumerable<Quiz> SearchByIsActive(
            IEnumerable<Quiz> quizzes, bool? isActive
        )
        {
            if (isActive == null)
            {
                return quizzes;
            }

            return quizzes.Where(q => q.IsActive == isActive).ToList();
        }

        private async Task MapAndAddQuestionsAsync(long quizId, QuizAddRequestDTO quizAddRequestDTO)
        {
            if (quizAddRequestDTO.Questions == null || quizAddRequestDTO.Questions.Count == 0)
            {
                throw new ValidationException("At least one question is required.");
            }

            foreach (var questionDTO in quizAddRequestDTO.Questions)
            {
                var question = questionMapper.MapQuestionAddRequestQuestion(questionDTO);
                if (question == null)
                {
                    throw new Exception("Failed to map question from DTO.");
                }
                var addedQuestion = await _questionRepository.Add(question);

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
                if (addedQuestion == null)
                {
                    throw new Exception("Failed to add question");
                }
            }
        }
        

        
   }
}