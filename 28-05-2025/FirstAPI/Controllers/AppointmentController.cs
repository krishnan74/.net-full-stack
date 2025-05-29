using Microsoft.AspNetCore.Mvc;
using FirstAPI.Models;
using FirstAPI.Services;
using FirstAPI.Interfaces;

namespace FirstAPI.Controllers{

    [ApiController]
    [Route("/api/[controller]")]

    public class AppointmentController : ControllerBase{
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public ActionResult<int> PostAppointment([FromBody] Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest("Appointment cannot be null");
            }

            var result = _appointmentService.AddAppointment(appointment);
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetAppointment), new { id = result }, result);
            }
            return BadRequest("Failed to create appointment");
        }

        [HttpGet]
        public ActionResult<List<Appointment>> GetAppointments([FromQuery] AppointmentSearchModel appointmentSearchModel)
        {
            var appointments = _appointmentService.SearchAppointment(appointmentSearchModel);
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound("No appointments found");
            }
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public ActionResult<Appointment> GetAppointment(int id)
        {
            var appointment = _appointmentService.GetAppointmentById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        [HttpPut]
        public ActionResult<Appointment> PutAppointment([FromBody] Appointment appointment)
        {
            if (appointment == null || appointment.AppointmentNumber <= 0)
            {
                return BadRequest("Invalid appointment data");
            }

            var updatedAppointment = _appointmentService.UpdateAppointment(appointment);
            if (updatedAppointment == null)
            {
                return NotFound();
            }
            return Ok(updatedAppointment);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAppointment(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid appointment ID");
            }

            _appointmentService.DeleteAppointment(id);
            return NoContent();
        }
    }
}