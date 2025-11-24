using API_SGHSS.DTOs.PatientDTOs;
using API_SGHSS.Models;
using API_SGHSS.Repositories.Interfaces;
using AutoMapper;
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
        private readonly ILogger<PatientController> _logger;
        private readonly IMapper _mapper;

        public PatientController(IPatientRepository repository, ILogger<PatientController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper; 
        }

        [HttpGet]
        public ActionResult<IEnumerable<PatientDTO>> GetAll()
        {

            _logger.LogInformation("Buscando todos os Pacientes Registrados");
            var patients = _repository.GetPatients();

            _logger.LogInformation("Retornando todos os Pacientes Registrados");
            var patientsDTO = _mapper.Map<IEnumerable<PatientDTO>>(patients);
            return Ok(patientsDTO);
        }

        [HttpGet("{id:int}", Name = "ObterPatient")]
        public ActionResult<PatientDTO> Get(int id)
        {
            _logger.LogInformation($"Buscando o paciente com Id = {id}");
            var patient = _repository.GetPatient(id);

            if (patient is null)
            {
                _logger.LogWarning($"Paciente com o id igual a {id} não encontrado");
                return NotFound($"Paciente com o id igual a {id} não encontrado");
            }

            _logger.LogInformation($"Retornando Paciente do id = {id}");
            var patientDTO = _mapper.Map<PatientDTO>(patient);
            return Ok(patientDTO);
        }

        [HttpPost]
        public ActionResult<PatientDTO> Post(PatientCreateDTO patientCreateDTO)
        {
            var patient = _mapper.Map<Patient>(patientCreateDTO);

            _logger.LogInformation("Criando um novo paciente");
            var newPatient = _repository.Create(patient);

            var newPatientDTO = _mapper.Map<PatientDTO>(newPatient);

            _logger.LogInformation("retornando o novo paciente");
            return new CreatedAtRouteResult("ObterPatient", new { id = newPatientDTO.Id }, newPatientDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<PatientDTO> Put(int id, PatientUpdateDTO patientUpdateDTO)
        {
            if (id != patientUpdateDTO.Id)
                return BadRequest("Dados para atualizar um paciente está incorreto");

            var existingPatient = _repository.GetPatient(id);

            if (existingPatient is null)
                return NotFound($"Paciente com ID igual a {id} não encontrado.");

            _mapper.Map(patientUpdateDTO, existingPatient);

            _logger.LogInformation("atualizando o paciente");
            var updatingPatient = _repository.Update(existingPatient);

            var updatingPatientDTO = _mapper.Map<PatientDTO>(existingPatient);

            _logger.LogInformation("Retornando o paciente com os dados atualizados");
            return Ok(updatingPatientDTO);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation("Obtendo id do paciente para deletá-lo");
            var patient = _repository.GetPatient(id);

            if (patient is null)
            {
                _logger.LogWarning($"Paciente com id igual a {id} não encontrado");
                return NotFound($"Paciente com o id igual a {id} não encontrado");
            }

            _logger.LogInformation("Removendo o paciente do banco de dados e mostrando as informações do paciente removido");
            _repository.Delete(id);
            return NoContent();
        }
    }
}
