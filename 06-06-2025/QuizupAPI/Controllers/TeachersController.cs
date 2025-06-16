using Microsoft.AspNetCore.Mvc;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Teacher;

namespace QuizupAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        /// <summary>
        /// Get all teachers
        /// </summary>
        /// <returns>List of all teachers</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Teacher>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllTeachers()
        {
            try
            {
                var teachers = await _teacherService.GetAllTeachersAsync();
                return Ok(teachers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving teachers. " + ex.Message });
            }
        }

        /// <summary>
        /// Get teacher by ID
        /// </summary>
        /// <param name="id">Teacher ID</param>
        /// <returns>Teacher information</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Teacher), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTeacherById(long id)
        {
            try
            {
                var teacher = await _teacherService.GetTeacherByIdAsync(id);
                return Ok(teacher);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the teacher. " + ex.Message });
            }
        }

        /// <summary>
        /// Get teachers based on pagination
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Number of teachers per page</param>
        /// <returns>List of teachers with pagination</returns>
        [HttpGet("pagination")]
        [ProducesResponseType(typeof(IEnumerable<Teacher>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTeachersPagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest(new { message = "Page number and size must be greater than zero." });
                }

                var teachers = await _teacherService.GetTeachersPaginationAsync(pageNumber, pageSize);
                return Ok(teachers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving teachers. " + ex.Message });
            }
        }

        /// <summary>
        /// Create a new teacher
        /// </summary>
        /// <param name="teacherDto">Teacher information</param>
        /// <returns>Created teacher</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Teacher), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTeacher([FromBody] TeacherAddRequestDTO teacherDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var teacher = await _teacherService.AddTeacherAsync(teacherDto);
                return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.Id }, teacher);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the teacher. " + ex.Message });
            }
        }

        /// <summary>
        /// Update teacher information
        /// </summary>
        /// <param name="id">Teacher ID</param>
        /// <param name="teacherDto">Updated teacher information</param>
        /// <returns>Updated teacher</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Teacher), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> UpdateTeacher(long id, [FromBody] TeacherUpdateRequestDTO teacherDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var teacher = await _teacherService.UpdateTeacherAsync(id, teacherDto);
                return Ok(teacher);
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
                return StatusCode(500, new { message = "An error occurred while updating the teacher. " + ex.Message });
            }
        }

        /// <summary>
        /// Delete a teacher
        /// </summary>
        /// <param name="id">Teacher ID</param>
        /// <returns>Deleted teacher</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Teacher), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTeacher(long id)
        {
            try
            {
                var teacher = await _teacherService.DeleteTeacherAsync(id);
                return Ok(teacher);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the teacher. " + ex.Message });
            }
        }

        /// <summary>
        /// Start a quiz
        /// </summary>
        /// <param name="teacherId">Teacher ID</param>
        /// <param name="quizId">Quiz ID</param>
        /// <returns>Updated quiz</returns>
        [HttpPost("{teacherId}/quizzes/{quizId}/start")]
        [ProducesResponseType(typeof(Quiz), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> StartQuiz(long teacherId, long quizId)
        {
            try
            {
                var quiz = await _teacherService.StartQuizAsync(teacherId, quizId);
                return Ok(quiz);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while starting the quiz. " + ex.Message });
            }
        }

        /// <summary>
        /// End a quiz
        /// </summary>
        /// <param name="teacherId">Teacher ID</param>
        /// <param name="quizId">Quiz ID</param>
        /// <returns>Updated quiz</returns>
        [HttpPost("{teacherId}/quizzes/{quizId}/end")]
        [ProducesResponseType(typeof(Quiz), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> EndQuiz(long teacherId, long quizId)
        {
            try
            {
                var quiz = await _teacherService.EndQuizAsync(teacherId, quizId);
                return Ok(quiz);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while ending the quiz. " + ex.Message });
            }
        }

        /// <summary>
        /// Get comprehensive quiz summary for a teacher
        /// </summary>
        /// <param name="id">Teacher ID</param>
        /// <param name="startDate">Start date for filtering ( Optional ) (format: yyyy-MM-dd)</param>
        /// <param name="endDate">End date for filtering ( Optional ) (format: yyyy-MM-dd)</param>
        /// <returns>Teacher quiz summary with performance analytics</returns>
        [HttpGet("{id}/summary")]
        [ProducesResponseType(typeof(TeacherSummaryDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTeacherQuizSummary(long id, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var summary = await _teacherService.GetTeacherQuizSummaryAsync(id, startDate, endDate);
                return Ok(summary);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving teacher quiz summary. " + ex.Message });
            }
        }
    }
} 