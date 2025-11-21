using API_SGHSS.Models;
using API_SGHSS.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_SGHSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _repository;

        public DoctorController(IDoctorRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Doctor>> Get()
        {
            var doctors = _repository.GetDoctors();
            return Ok(doctors);
        }

        [HttpGet("{id:int}", Name ="ObterDoctor")]
        public ActionResult<Doctor> Get(int id)
        {
            var doctor = _repository.GetDoctor(id);

            if (doctor is null)
            {
                return NotFound($"Médico com o id igual a {id} não encontrado");
            }

            return Ok(doctor);
        }

        [HttpPost]
        public ActionResult Post(Doctor doctor)
        {
            if (doctor is null)
            {
                return BadRequest("Dados ao cadastrar um novo médico estão inválidos");
            }

            var newDoctor = _repository.Create(doctor);
            return new CreatedAtRouteResult("ObterDoctor", new { id = newDoctor.Id }, newDoctor);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Doctor doctor)
        {
            if(id != doctor.Id)
                return BadRequest("Dados para buscar um médico estão inválidos");

            _repository.Update(doctor);
            return Ok(doctor);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Put(int id)
        {
            var doctor = _repository.GetDoctor(id);

            if (doctor is null)
                return BadRequest($"Médico com o id igual a {id} não encontrado");

            var removedDoctor = _repository.Delete(id);
            return Ok(removedDoctor);
        }
    }
}
