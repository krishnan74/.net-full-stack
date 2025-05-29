using Microsoft.AspNetCore.Mvc;
using FirstAPI.Models;

[ApiController]
[Route("/api/[controller]")]

public class DoctorController: ControllerBase{
    static List<Doctor> doctors = new List<Doctor>{
        new Doctor{Id = 101, Name = "Grey"},
        new Doctor{Id = 102, Name = "Mark"},
    };

    [HttpGet]
    public ActionResult<IEnumerable<Doctor>> GetDoctors(){
        return Ok(doctors);
    }

    [HttpGet("{id}")]
    public ActionResult<Doctor> GetDoctor(int id){
        var doctor = doctors.FirstOrDefault(d => d.Id == id);
        if (doctor == null) {
            return NotFound();
        }
        return Ok(doctor);
    }

    [HttpPost]
    public ActionResult<Doctor> PostDoctor([FromBody] Doctor doctor){
        doctors.Add(doctor);
        return Created("", doctor);
    }

    [HttpPut("{id}")]
    public ActionResult<Doctor> PutDoctor([FromBody] Doctor doctor){
        var existingDoctor = doctors.FirstOrDefault(d => d.Id == doctor.Id);
        if (existingDoctor == null) {
            return NotFound();
        }
        existingDoctor.Name = doctor.Name;
        return Ok(existingDoctor);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteDoctor(int id){
        var doctor = doctors.FirstOrDefault(d => d.Id == id);
        if (doctor == null) {
            return NotFound();
        }
        doctors.Remove(doctor);
        return NoContent();
    }

}