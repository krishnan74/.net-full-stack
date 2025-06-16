using Microsoft.AspNetCore.Mvc;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Quiz;
using QuizupAPI.Models.DTOs.Question;

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
        [ProducesResponseType(typeof(IEnumerable<Quiz>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllQuizzes()
        {
            try
            {
                var quizzes = await _quizService.GetAllQuizzesAsync();
                return Ok(quizzes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving quizzes. " +  ex.Message });
            }
        }

        /// <summary>
        /// Get quiz by ID
        /// </summary>
        /// <param name="id">Quiz ID</param>
        /// <returns>Quiz information</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Quiz), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetQuizById(long id)
        {
            try
            {
                var quiz = await _quizService.GetQuizByIdAsync(id);
                return Ok(quiz);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the quiz. " + ex.Message });
            }
        }
        
        // <summary>
        /// Get quizzes based on pagination
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Number of quizzes per page</param>
        /// <returns>List of quizzes with pagination</returns>
        [HttpGet("pagination")]
        [ProducesResponseType(typeof(IEnumerable<Quiz>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetQuizzesPagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest(new { message = "Page number and size must be greater than zero." });
                }

                var quizzes = await _quizService.GetQuizzesPaginationAsync(pageNumber, pageSize);
                return Ok(quizzes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving quizzes. " + ex.Message });
            }
        }

        /// <summary>
        /// Create a new quiz
        /// </summary>
        /// <param name="quizDto">Quiz information</param>
        /// <returns>Created quiz</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Quiz), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizAddRequestDTO quizDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var quiz = await _quizService.AddQuizAsync(quizDto);
                return CreatedAtAction(nameof(GetQuizById), new { id = quiz.Id }, quiz);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the quiz. " + ex.Message });
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
        [ProducesResponseType(typeof(Quiz), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> UpdateQuiz(long id, [FromQuery] long teacherId, [FromBody] QuizUpdateRequestDTO quizDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var quiz = await _quizService.UpdateQuizAsync(id, teacherId, quizDto);
                return Ok(quiz);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the quiz. " + ex.Message });
            }
        }

        /// <summary>
        /// Delete a quiz
        /// </summary>
        /// <param name="id">Quiz ID</param>
        /// <param name="teacherId">Teacher ID</param>
        /// <returns>Deleted quiz</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Quiz), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteQuiz(long id, [FromQuery] long teacherId)
        {
            try
            {
                var quiz = await _quizService.DeleteQuizAsync(id, teacherId);
                return Ok(quiz);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the quiz. " + ex.Message });
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
        [ProducesResponseType(typeof(Question), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddQuestionToQuiz(long quizId, [FromQuery] long teacherId, [FromBody] QuestionAddRequestDTO questionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var question = await _quizService.AddQuestionToQuizAsync(quizId, teacherId, questionDto);
                return CreatedAtAction(nameof(GetQuizById), new { id = quizId }, question);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the question to the quiz. " + ex.Message });
            }
        }

        /// <summary>
        /// Update a question in a quiz
        /// </summary>
        /// <param name="quizId">Quiz ID</param>
        /// <param name="teacherId">Teacher ID</param>
        /// <param name="questionDto">Updated question information</param>
        /// <returns>Updated question</returns>
        [HttpPut("{quizId}/questions")]
        [ProducesResponseType(typeof(Question), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> UpdateQuestion(long quizId, [FromQuery] long teacherId, [FromBody] QuestionUpdateRequestDTO questionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var question = await _quizService.UpdateQuestionAsync(quizId, teacherId, questionDto);
                return Ok(question);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the question. " + ex.Message });
            }
        }
    }
} 