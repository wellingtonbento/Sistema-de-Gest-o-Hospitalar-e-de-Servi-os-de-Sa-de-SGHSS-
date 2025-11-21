using API_SGHSS.Models;
using API_SGHSS.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_SGHSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentController(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Appointment>> Get()
        {
            var appointments = _repository.GetAppointments();

            return Ok(appointments);
        }

        [HttpGet("{id:int}", Name = "ObterAppointment")]
        public ActionResult<Appointment> Get(int id)
        {
            var appointment = _repository.GetAppointment(id);

            if(appointment is null)
                return NotFound($"Consulta com o id igual a {id} não encontrado");

            return Ok(appointment);
        }

        [HttpPost]
        public ActionResult Post(Appointment appointment)
        {
            if (appointment is null)
                return BadRequest("Dados ao cadastrar uma nova consulta estão inválidos");

            var newAppointment = _repository.Create(appointment);
            return new CreatedAtRouteResult("ObterAppointment", new { id = newAppointment.Id }, newAppointment);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Appointment appointment)
        {
            if (id != appointment.Id)
                return BadRequest("Dados para buscar uma consulta estão inválidos");

            _repository.Update(appointment);
            return Ok(appointment);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var appointment = _repository.GetAppointment(id);

            if (appointment is null)
                return NotFound($"Consulta com o id igual a {id} não encontrado");

            var removedAppointments = _repository.Delete(id);
            return Ok(removedAppointments);
        }
    }
}
