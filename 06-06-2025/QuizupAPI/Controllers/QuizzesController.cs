using Microsoft.AspNetCore.Mvc;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Quiz;
using QuizupAPI.Models.DTOs.Question;
using QuizupAPI.Models.DTOs.Response;
using QuizupAPI.Models.SearchModels;
using Microsoft.AspNetCore.Authorization;


namespace QuizupAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizzesController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        /// <summary>
        /// Get all quizzes
        /// </summary>
        /// <returns>List of all quizzes</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Quiz>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetAllQuizzes()
        {
            try
            {
                var quizzes = await _quizService.GetAllQuizzesAsync();
                return Ok(ApiResponse<IEnumerable<Quiz>>.SuccessResponse(quizzes, "Quizzes fetched successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving quizzes. " + ex.Message));
            }
        }

        /// <summary>
        /// Get quiz by ID
        /// </summary>
        /// <param name="id">Quiz ID</param>
        /// <returns>Quiz information</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Quiz>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetQuizById(long id)
        {
            try
            {
                var quiz = await _quizService.GetQuizByIdAsync(id);
                return Ok(ApiResponse<Quiz>.SuccessResponse(quiz, "Quiz fetched successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving the quiz. " + ex.Message));
            }
        }
        
        /// <summary>
        /// Get quizzes based on pagination
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Number of quizzes per page</param>
        /// <returns>List of quizzes with pagination</returns>
        [HttpGet("pagination")]
        [ProducesResponseType(typeof(PaginatedResponse<IEnumerable<Quiz>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetQuizzesPagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Page number and size must be greater than zero."));
                }

                var result = await _quizService.GetQuizzesPaginationAsync(pageNumber, pageSize);
                var allQuizzes = await _quizService.GetAllQuizzesAsync();
                var totalRecords = allQuizzes.Count();
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                var paginationInfo = new PaginationInfo
                {
                    TotalRecords = totalRecords,
                    Page = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(PaginatedResponse<IEnumerable<Quiz>>.SuccessResponse(result, paginationInfo, "Quizzes fetched successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving quizzes. " + ex.Message));
            }
        }

        /// <summary>
        /// Search quizzes based on various criteria
        /// </summary>
        /// <param name="quizSearchModel">Search criteria</param>
        /// <returns>List of quizzes matching the search criteria</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Quiz>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> SearchQuizzes([FromQuery] QuizSearchModel quizSearchModel)
        {
            try
            {
                if (quizSearchModel == null)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Search criteria cannot be null."));
                }
                var quizzes = await _quizService.SearchQuiz(quizSearchModel);
                if (quizzes == null || !quizzes.Any())
                {
                    return Ok(ApiResponse<IEnumerable<Quiz>>.SuccessResponse(new List<Quiz>(), "No quizzes found matching the search criteria"));
                }

                return Ok(ApiResponse<IEnumerable<Quiz>>.SuccessResponse(quizzes, "Quizzes fetched successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while searching for quizzes. " + ex.Message));
            }
        }

        /// <summary>
        /// Create a new quiz
        /// </summary>
        /// <param name="quizDto">Quiz information</param>
        /// <returns>Created quiz</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Quiz>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizAddRequestDTO quizDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
                }

                var quiz = await _quizService.AddQuizAsync(quizDto);
                return CreatedAtAction(nameof(GetQuizById), new { id = quiz.Id }, 
                    ApiResponse<Quiz>.SuccessResponse(quiz, "Quiz created successfully"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the quiz. " + ex.Message));
            }
        }

        /// <summary>
        /// Update quiz information
        /// </summary>
        /// <param name="id">Quiz ID</param>
        /// <param name="teacherId">Teacher ID</param>
        /// <param name="quizDto">Updated quiz information</param>
        /// <returns>Updated quiz</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Quiz>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> UpdateQuiz(long id, [FromQuery] long teacherId, [FromBody] QuizUpdateRequestDTO quizDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
                }

                var quiz = await _quizService.UpdateQuizAsync(id, teacherId, quizDto);
                return Ok(ApiResponse<Quiz>.SuccessResponse(quiz, "Quiz updated successfully"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the quiz. " + ex.Message));
            }
        }

        /// <summary>
        /// Delete a quiz
        /// </summary>
        /// <param name="id">Quiz ID</param>
        /// <param name="teacherId">Teacher ID</param>
        /// <returns>Deleted quiz</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Quiz>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteQuiz(long id, [FromQuery] long teacherId)
        {
            try
            {
                var quiz = await _quizService.DeleteQuizAsync(id, teacherId);
                return Ok(ApiResponse<Quiz>.SuccessResponse(quiz, "Quiz deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the quiz. " + ex.Message));
            }
        }

        /// <summary>
        /// Add a question to a quiz
        /// </summary>
        /// <param name="quizId">Quiz ID</param>
        /// <param name="teacherId">Teacher ID</param>
        /// <param name="questionDto">Question information</param>
        /// <returns>Created question</returns>
        [HttpPost("{quizId}/questions")]
        [ProducesResponseType(typeof(ApiResponse<Question>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddQuestionToQuiz(long quizId, [FromQuery] long teacherId, [FromBody] QuestionAddRequestDTO questionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
                }

                var question = await _quizService.AddQuestionToQuizAsync(quizId, teacherId, questionDto);
                return CreatedAtAction(nameof(AddQuestionToQuiz), new { quizId, teacherId }, 
                    ApiResponse<Question>.SuccessResponse(question, "Question added successfully"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while adding the question. " + ex.Message));
            }
        }

        /// <summary>
        /// Update a question in a quiz
        /// </summary>
        /// <param name="questionId">Question ID</param>
        /// <param name="teacherId">Teacher ID</param>
        /// <param name="questionDto">Updated question information</param>
        /// <returns>Updated question</returns>
        [HttpPut("{questionId}/questions")]
        [ProducesResponseType(typeof(ApiResponse<Question>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> UpdateQuestion(long questionId, [FromQuery] long teacherId, [FromBody] QuestionUpdateRequestDTO questionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
                }

                var question = await _quizService.UpdateQuestionAsync(questionId, teacherId, questionDto);
                return Ok(ApiResponse<Question>.SuccessResponse(question, "Question updated successfully"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the question. " + ex.Message));
            }
        }
    }
} 