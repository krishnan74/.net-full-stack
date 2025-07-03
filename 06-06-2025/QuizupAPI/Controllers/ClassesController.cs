using Microsoft.AspNetCore.Mvc;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Response;
using QuizupAPI.Models.DTOs.Classe;
using Microsoft.AspNetCore.Authorization;

namespace QuizupAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }

        /// <summary>
        /// Get all classes
        /// </summary>
        /// <returns>List of classes</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Classe>>), 200)]
        public async Task<IActionResult> GetAllClasses()
        {
            try
            {
                var classes = await _classService.GetAllClassesAsync();
                return Ok(ApiResponse<IEnumerable<Classe>>.SuccessResponse(classes, "Classes fetched successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching classes", ex.Message));
            }
        }

        /// <summary>
        /// Get a class by ID
        /// </summary>
        /// <param name="id">Classe ID</param>
        /// <returns>Classe information</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Classe>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetClassById(long id)
        {
            try
            {
                var classObj = await _classService.GetClassByIdAsync(id);
                return Ok(ApiResponse<Classe>.SuccessResponse(classObj, "Classe fetched successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching the class", ex.Message));
            }
        }

        /// <summary>
        /// Create a new class
        /// </summary>
        /// <param name="className">Name of the class</param>
        /// <param name="subjectIds">List of subject IDs ( optional )</param>
        /// <returns>Created class</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Classe>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> CreateClass([FromBody] ClassDTO classDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(classDto.ClassName))
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Classe name is required."));
                }
                var classObj = await _classService.AddClassAsync(classDto.ClassName, classDto.SubjectIds);
                return CreatedAtAction(nameof(GetClassById), new { id = classObj.Id }, ApiResponse<Classe>.SuccessResponse(classObj, "Classe created successfully"));
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
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the class. " + ex.Message));
            }
        }

        /// <summary>
        /// Update class information
        /// </summary>
        /// <param name="id">Classe ID</param>
        /// <param name="className">New class name</param>
        /// <returns>Updated class</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Classe>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateClass(long id, [FromBody] string className)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(className))
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Classe name is required."));
                }
                var updatedClass = await _classService.UpdateClassAsync(id, className);
                return Ok(ApiResponse<Classe>.SuccessResponse(updatedClass, "Classe updated successfully"));
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
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the class. " + ex.Message));
            }
        }

        /// <summary>
        /// Delete a class by ID
        /// </summary>
        /// <param name="id">Classe ID</param>
        /// <returns>Deleted class</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Classe>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClass(long id)
        {
            try
            {
                var deletedClass = await _classService.DeleteClassAsync(id);
                return Ok(ApiResponse<Classe>.SuccessResponse(deletedClass, "Classe deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the class. " + ex.Message));
            }
        }

        /// <summary>
        /// Get subjects by class ID
        /// </summary>
        /// <param name="classId">Classe ID</param>
        /// <returns>List of subjects associated with the class</returns>
        [HttpGet("{classId}/subjects")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Subject>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Admin, Teacher, Student")]
        public async Task<IActionResult> GetSubjectsByClassId(long classId)
        {
            try
            {
                var subjects = await _classService.GetSubjectsByClassIdAsync(classId);
                return Ok(ApiResponse<IEnumerable<Subject>>.SuccessResponse(subjects, "Subjects fetched successfully for class"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching subjects for class. " + ex.Message));
            }
        }

        /// <summary>
        /// Add a subject to a class
        /// </summary>
        [HttpPost("{classId}/subjects/{subjectId}")]
        [ProducesResponseType(typeof(ApiResponse<Classe>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSubjectToClass(long classId, long subjectId)
        {
            try
            {
                var updatedClass = await _classService.AddSubjectToClassAsync(classId, subjectId);
                return Ok(ApiResponse<Classe>.SuccessResponse(updatedClass, "Subject added to class successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while adding subject to class. " + ex.Message));
            }
        }
    }
}
