
using System.Threading.Tasks;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.Appointment;
using FirstAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace FirstAPI.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        [Authorize] 
        public async Task<ActionResult<Appointment>> PostAppointment([FromBody] AppointmentAddRequestDTO appointment)
        {
            try
            {
                var newAppointment = await _appointmentService.AddAppointment(appointment);
                if (newAppointment != null)
                    return Created("", newAppointment);
                return BadRequest("Unable to process request at this moment");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{appointmentNumber}")]
        [Authorize(Policy = "AtLeastHas3YearsExperience")]
        public async Task<ActionResult<Appointment>> CancelAppointment(string appointmentNumber)
        {
            try
            {
                var cancelledAppointment = await _appointmentService.CancelAppointment(appointmentNumber);
                if (cancelledAppointment != null)
                    return Ok(cancelledAppointment);
                return NotFound("Appointment not found");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}