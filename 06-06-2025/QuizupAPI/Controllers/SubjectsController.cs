using Microsoft.AspNetCore.Mvc;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Response;
using QuizupAPI.Models.DTOs.Subject;

using Microsoft.AspNetCore.Authorization;


namespace QuizupAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        /// <summary>
        /// Get all subjects
        /// </summary>
        /// <returns>List of subjects</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Subject>>), 200)]
        public async Task<IActionResult> GetAllSubjects()
        {
            try
            {
                var subjects = await _subjectService.GetAllSubjectsAsync();
                return Ok(ApiResponse<IEnumerable<Subject>>.SuccessResponse(subjects, "Subjects fetched successfully"));
            }

            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching subjects", ex.Message));
            }
        }

        /// <summary>
        /// Get a subject by ID
        /// </summary>
        /// <param name="id">Subject ID</param>
        /// <returns>Subject information</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Student>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetSubjectById(long id)
        {
            try
            {
                var subject = await _subjectService.GetSubjectByIdAsync(id);
                return Ok(ApiResponse<Subject>.SuccessResponse(subject, "Subject fetched successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching the subject", ex.Message));
            }
        }

        /// <summary>
        /// Create a new subject
        /// </summary>
        /// <param name="subjectDto">Subject information</param>
        /// <returns>Created subject</returns>
        [HttpPost]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Student>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectDTO subjectDto)
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



                var subject = await _subjectService.AddSubjectAsync(subjectDto);
                return CreatedAtAction(nameof(GetSubjectById), new { id = subject.Id }, ApiResponse<Subject>.SuccessResponse(subject, "Subject created successfully"));
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
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the subject. " + ex.Message));
            }
        }

        /// <summary>
        /// Update subject information
        /// </summary>
        /// <param name="id">Subject ID</param>
        /// <param name="subjectDto">Updated subject information</param>
        /// <returns>Updated subject</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Student>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSubject(long id, [FromBody] SubjectDTO subjectDto)
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

                var updatedSubject = await _subjectService.UpdateSubjectAsync(id, subjectDto);

                return Ok(ApiResponse<Subject>.SuccessResponse(updatedSubject, "Subject updated successfully"));
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
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the subject. " + ex.Message));
            }
        }

        /// <summary>
        /// Delete a subject by ID
        /// </summary>
        /// <param name="id">Subject ID</param>
        /// <returns>Deleted subject</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Student>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSubject(long id)
        {
            try
            {
                var deletedSubject = await _subjectService.DeleteSubjectAsync(id);

                return Ok(ApiResponse<Subject>.SuccessResponse(deletedSubject, "Subject deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the subject. " + ex.Message));
            }
        }

        

    }
}