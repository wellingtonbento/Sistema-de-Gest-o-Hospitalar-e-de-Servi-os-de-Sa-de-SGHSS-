using API_SGHSS.Models;
using API_SGHSS.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_SGHSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _repository;

        public PatientController(IPatientRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Patient>> Get()
        {
            var patients = _repository.GetPatients();
            return Ok(patients);
        }

        [HttpGet("{id:int}", Name = "ObterPatient")]
        public ActionResult<Patient> Get(int id)
        {
            var patient = _repository.GetPatient(id);

            if (patient is null)
            {
                return NotFound($"Paciente com o id igual a {id} não encontrado");
            }

            return Ok(patient);
        }

        [HttpPost]
        public ActionResult Post(Patient patient)
        {
            if (patient is null)
                return BadRequest("Dados ao cadastrar um novo paciente estão inválidos");

            var newPatient = _repository.Create(patient);

            return new CreatedAtRouteResult("ObterPatient", new { id = newPatient.Id }, newPatient);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Patient patient)
        {
            if (id != patient.Id)
                return BadRequest("Dados para buscar um paciente estão inválidos");

            _repository.Update(patient);
            return Ok(patient);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var patient = _repository.GetPatient(id);

            if (patient is null)
                return NotFound($"Patient com o id igual a {id} não encontrado");

            var removedPatient = _repository.Delete(id);
            return Ok(removedPatient);
        }
    }
}
