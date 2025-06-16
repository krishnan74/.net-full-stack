using Microsoft.AspNetCore.Mvc;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Student;
using QuizupAPI.Models.DTOs.QuizSubmission;
using QuizupAPI.Models.DTOs.Response;
using Microsoft.AspNetCore.Authorization;


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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Student>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return Ok(ApiResponse<IEnumerable<Student>>.SuccessResponse(students, "Students fetched successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving students. " + ex.Message));
            }
        }

        /// <summary>
        /// Get student by ID
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>Student information</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Student>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetStudentById(long id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                return Ok(ApiResponse<Student>.SuccessResponse(student, "Student fetched successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving the student. " + ex.Message));
            }
        }

        /// <summary>
        /// Get students based on pagination
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Number of students per page</param>
        /// <returns>List of students with pagination</returns>
        [HttpGet("pagination")]
        [ProducesResponseType(typeof(PaginatedResponse<IEnumerable<Student>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetStudentsPagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Page number and size must be greater than zero."));
                }

                var result = await _studentService.GetStudentsPaginationAsync(pageNumber, pageSize);
                var allStudents = await _studentService.GetAllStudentsAsync();
                var totalRecords = allStudents.Count();
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                var paginationInfo = new PaginationInfo
                {
                    TotalRecords = totalRecords,
                    Page = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(PaginatedResponse<IEnumerable<Student>>.SuccessResponse(result, paginationInfo, "Students fetched successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving students. " + ex.Message));
            }
        }

        /// <summary>
        /// Create a new student
        /// </summary>
        /// <param name="studentDto">Student information</param>
        /// <returns>Created student</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Student>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateStudent([FromBody] StudentAddRequestDTO studentDto)
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

                var student = await _studentService.AddStudentAsync(studentDto);
                return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, 
                    ApiResponse<Student>.SuccessResponse(student, "Student created successfully"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the student. " + ex.Message));
            }
        }

        /// <summary>
        /// Update student information
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="studentDto">Updated student information</param>
        /// <returns>Updated student</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Student>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Student, Admin")]
        public async Task<IActionResult> UpdateStudent(long id, [FromBody] StudentUpdateRequestDTO studentDto)
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

                var student = await _studentService.UpdateStudentAsync(id, studentDto);
                return Ok(ApiResponse<Student>.SuccessResponse(student, "Student updated successfully"));
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
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the student. " + ex.Message));
            }
        }

        /// <summary>
        /// Delete a student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>Deleted student</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Student>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            try
            {
                var student = await _studentService.DeleteStudentAsync(id);
                return Ok(ApiResponse<Student>.SuccessResponse(student, "Student deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the student. " + ex.Message));
            }
        }

        /// <summary>
        /// Get all quiz submissions for a student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>List of quiz submissions</returns>
        [HttpGet("{id}/submissions")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<QuizSubmission>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetStudentSubmissions(long id)
        {
            try
            {
                var submissions = await _studentService.GetSubmissionsByStudentIdAsync(id);
                return Ok(ApiResponse<IEnumerable<QuizSubmission>>.SuccessResponse(submissions, "Student submissions fetched successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving student submissions. " + ex.Message));
            }
        }

        /// <summary>
        /// Start a quiz for a student
        /// </summary>
        /// <param name="studentId">Student ID</param>
        /// <param name="quizId">Quiz ID</param>
        /// <returns>Quiz submission</returns>
        [HttpPost("{studentId}/quizzes/{quizId}/start")]
        [ProducesResponseType(typeof(ApiResponse<QuizSubmission>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StartQuiz(long studentId, long quizId)
        {
            try
            {
                var submission = await _studentService.StartQuizAsync(studentId, quizId);
                return Ok(ApiResponse<QuizSubmission>.SuccessResponse(submission, "Quiz started successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while starting the quiz. " + ex.Message));
            }
        }

        /// <summary>
        /// Submit quiz answers
        /// </summary>
        /// <param name="studentId">Student ID</param>
        /// <param name="quizSubmissionId">Quiz submission ID</param>
        /// <param name="submission">Quiz submission with answers</param>
        /// <returns>Submitted quiz submission</returns>
        [HttpPost("{studentId}/submissions/{quizSubmissionId}/submit")]
        [ProducesResponseType(typeof(ApiResponse<QuizSubmission>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitQuiz(long studentId, long quizSubmissionId, [FromBody] QuizSubmissionDTO submission)
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

                var result = await _studentService.SubmitQuizAsync(studentId, quizSubmissionId, submission);
                return Ok(ApiResponse<QuizSubmission>.SuccessResponse(result, "Quiz submitted successfully"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while submitting the quiz. " + ex.Message));
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
        [ProducesResponseType(typeof(ApiResponse<QuizSubmission>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SaveQuizAnswers(long studentId, long quizSubmissionId, [FromBody] QuizSubmissionDTO submission)
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

                var result = await _studentService.SaveAnswersAsync(studentId, quizSubmissionId, submission);
                return Ok(ApiResponse<QuizSubmission>.SuccessResponse(result, "Quiz answers saved successfully"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while saving quiz answers. " + ex.Message));
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
        [ProducesResponseType(typeof(ApiResponse<StudentSummaryDTO>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetStudentQuizSummary(long id, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var summary = await _studentService.GetStudentQuizSummaryAsync(id, startDate, endDate);
                return Ok(ApiResponse<StudentSummaryDTO>.SuccessResponse(summary, "Student quiz summary fetched successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving student quiz summary. " + ex.Message));
            }
        }
    }
} 