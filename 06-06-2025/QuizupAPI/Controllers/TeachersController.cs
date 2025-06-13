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
                return StatusCode(500, new { message = "An error occurred while retrieving teachers." });
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
                return StatusCode(500, new { message = "An error occurred while retrieving the teacher." });
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
                return StatusCode(500, new { message = "An error occurred while creating the teacher." });
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
                return StatusCode(500, new { message = "An error occurred while updating the teacher." });
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
                return StatusCode(500, new { message = "An error occurred while deleting the teacher." });
            }
        }

        /// <summary>
        /// Start a quiz (make it active for students)
        /// </summary>
        /// <param name="teacherId">Teacher ID</param>
        /// <param name="quizId">Quiz ID</param>
        /// <returns>Updated quiz</returns>
        [HttpPost("{teacherId}/quizzes/{quizId}/start")]
        [ProducesResponseType(typeof(Quiz), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
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
                return StatusCode(500, new { message = "An error occurred while starting the quiz." });
            }
        }

        /// <summary>
        /// End a quiz (make it inactive for students)
        /// </summary>
        /// <param name="teacherId">Teacher ID</param>
        /// <param name="quizId">Quiz ID</param>
        /// <returns>Updated quiz</returns>
        [HttpPost("{teacherId}/quizzes/{quizId}/end")]
        [ProducesResponseType(typeof(Quiz), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
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
                return StatusCode(500, new { message = "An error occurred while ending the quiz." });
            }
        }
    }
} 