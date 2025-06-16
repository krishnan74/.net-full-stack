using Microsoft.AspNetCore.Mvc;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Student;
using QuizupAPI.Models.DTOs.QuizSubmission;

namespace QuizupAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns>List of all students</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Student>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving students. " + ex.Message });
            }
        }

        /// <summary>
        /// Get student by ID
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>Student information</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Student), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetStudentById(long id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                return Ok(student);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the student. " + ex.Message });
            }
        }

        /// <summary>
        /// Get students based on pagination
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Number of students per page</param>
        /// <returns>List of students with pagination</returns>
        [HttpGet("pagination")]
        [ProducesResponseType(typeof(IEnumerable<Student>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetStudentsPagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest(new { message = "Page number and size must be greater than zero." });
                }

                var students = await _studentService.GetStudentsPaginationAsync(pageNumber, pageSize);
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving students. " + ex.Message });
            }
        }

        /// <summary>
        /// Create a new student
        /// </summary>
        /// <param name="studentDto">Student information</param>
        /// <returns>Created student</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Student), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateStudent([FromBody] StudentAddRequestDTO studentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var student = await _studentService.AddStudentAsync(studentDto);
                return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
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
                return StatusCode(500, new { message = "An error occurred while creating the student. " + ex.Message });
            }
        }

        /// <summary>
        /// Update student information
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="studentDto">Updated student information</param>
        /// <returns>Updated student</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Student), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateStudent(long id, [FromBody] StudentUpdateRequestDTO studentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var student = await _studentService.UpdateStudentAsync(id, studentDto);
                return Ok(student);
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
                return StatusCode(500, new { message = "An error occurred while updating the student. " + ex.Message });
            }
        }

        /// <summary>
        /// Delete a student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>Deleted student</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Student), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            try
            {
                var student = await _studentService.DeleteStudentAsync(id);
                return Ok(student);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the student. " + ex.Message });
            }
        }

        /// <summary>
        /// Get quiz submissions for a student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>List of quiz submissions</returns>
        [HttpGet("{id}/submissions")]
        [ProducesResponseType(typeof(IEnumerable<QuizSubmission>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetStudentSubmissions(long id)
        {
            try
            {
                var submissions = await _studentService.GetSubmissionsByStudentIdAsync(id);
                return Ok(submissions);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving student submissions. " + ex.Message });
            }
        }

        /// <summary>
        /// Start a quiz for a student
        /// </summary>
        /// <param name="studentId">Student ID</param>
        /// <param name="quizId">Quiz ID</param>
        /// <returns>Quiz submission</returns>
        [HttpPost("{studentId}/quizzes/{quizId}/start")]
        [ProducesResponseType(typeof(QuizSubmission), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> StartQuiz(long studentId, long quizId)
        {
            try
            {
                var submission = await _studentService.StartQuizAsync(studentId, quizId);
                return Ok(submission);
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
        /// Submit quiz answers
        /// </summary>
        /// <param name="studentId">Student ID</param>
        /// <param name="quizSubmissionId">Quiz submission ID</param>
        /// <param name="submission">Quiz submission with answers</param>
        /// <returns>Submitted quiz</returns>
        [HttpPost("{studentId}/submissions/{quizSubmissionId}/submit")]
        [ProducesResponseType(typeof(QuizSubmission), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SubmitQuiz(long studentId, long quizSubmissionId, [FromBody] QuizSubmissionDTO submission)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _studentService.SubmitQuizAsync(studentId, quizSubmissionId, submission);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while submitting the quiz. " + ex.Message });
            }
        }

        /// <summary>
        /// Save quiz answers without submitting
        /// </summary>
        /// <param name="studentId">Student ID</param>
        /// <param name="quizSubmissionId">Quiz submission ID</param>
        /// <param name="submission">Quiz submission with answers</param>
        /// <returns>Saved quiz submission</returns>
        [HttpPost("{studentId}/submissions/{quizSubmissionId}/save")]
        [ProducesResponseType(typeof(QuizSubmission), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SaveQuizAnswers(long studentId, long quizSubmissionId, [FromBody] QuizSubmissionDTO submission)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _studentService.SaveAnswersAsync(studentId, quizSubmissionId, submission);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving quiz answers. " + ex.Message });
            }
        }

        /// <summary>
        /// Get quiz summary for a student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="startDate">Start date for filtering ( Optional ) (format: yyyy-MM-dd)</param>
        /// <param name="endDate">End date for filtering ( Optional ) (format: yyyy-MM-dd)</param>
        /// <returns>Student quiz summary with performance analytics</returns>
        [HttpGet("{id}/summary")]
        [ProducesResponseType(typeof(StudentSummaryDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetStudentQuizSummary(long id, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var summary = await _studentService.GetStudentQuizSummaryAsync(id, startDate, endDate);
                return Ok(summary);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving student quiz summary. " + ex.Message });
            }
        }
    }
} 