
using System.Threading.Tasks;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.Patient;
using FirstAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace FirstAPI.Controllers
{


    [ApiController]
    [Route("/api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("by-diagnosis")]
        [Authorize]
        public async Task<ActionResult<ICollection<PatientsByDiagnosisResponseDTO>>> GetPatientsByDiagnosis([FromQuery] string diagnosis)
        {
            var result = await _patientService.GetPatientsByDiagnosis(diagnosis);
            if (result == null || !result.Any())
                return NotFound("No patients found with the specified diagnosis.");
            return Ok(result);
        }

        [HttpGet("by-name")]
        [Authorize]
        public async Task<ActionResult<Patient>> GetPatientByName([FromQuery] string name)
        {
            var patient = await _patientService.GetPatientByName(name);
            if (patient == null)
                return NotFound("Patient not found.");
            return Ok(patient);
        }
            
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient([FromBody] PatientAddRequestDTO patient)
        {
            try
            {
                var newPatient = await _patientService.AddPatient(patient);
                if (newPatient != null)
                    return Created("", newPatient);
                return BadRequest("Unable to process request at this moment");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}