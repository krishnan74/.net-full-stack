using Microsoft.AspNetCore.Mvc;
using FirstAPI.Models;

[ApiController]
[Route("/api/[controller]")]

public class PatientController: ControllerBase{
    static List<Patient> patients = new List<Patient>{
        new Patient{Id = 1, Name = "Harish", Age = 30, Diagnosis = "Flu"},
        new Patient{Id = 2, Name = "Vikash", Age = 25, Diagnosis = "Cold"},
    };

    [HttpGet]
    public ActionResult<IEnumerable<Patient>> GetPatients(){
        return Ok(patients);
    }

    [HttpGet("{id}")]
    public ActionResult<Patient> GetPatient(int id){
        var patient = patients.FirstOrDefault(p => p.Id == id);
        if (patient == null) {
            return NotFound();
        }
        return Ok(patient);
    }

    [HttpPost]
    public ActionResult<Patient> PostPatient([FromBody] Patient patient){
        patients.Add(patient);
        return Created("", patient);
    }

    [HttpPut("{id}")]
    public ActionResult<Patient> PutPatient([FromBody] Patient patient){
        var existingPatient = patients.FirstOrDefault(p => p.Id == patient.Id);
        if (existingPatient == null) {
            return NotFound();
        }
        existingPatient.Name = patient.Name;
        existingPatient.Age = patient.Age;
        existingPatient.Diagnosis = patient.Diagnosis;
        return Ok(existingPatient);
    }

    [HttpDelete("{id}")]
    public ActionResult DeletePatient(int id){
        var patient = patients.FirstOrDefault(p => p.Id == id);
        if (patient == null) {
            return NotFound();
        }
        patients.Remove(patient);
        return NoContent();
    }
}