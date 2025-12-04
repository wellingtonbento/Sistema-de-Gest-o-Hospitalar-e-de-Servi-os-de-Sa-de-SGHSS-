using API_SGHSS.DTOs.PatientDTOs;
using API_SGHSS.Models;
using API_SGHSS.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_SGHSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;
        private readonly ILogger<PatientController> _logger;
        private readonly IMapper _mapper;

        public PatientController(IPatientService service, ILogger<PatientController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper; 
        }

        /// <summary>
        /// Obtem uma lista de Paciente.
        /// </summary>
        /// <returns>Retorna 200 OK e uma lista de PacienteDTO.</returns>
        [HttpGet]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetAll()
        {
            _logger.LogInformation("Buscando todos os Pacientes Registrados");
            var patients = await _service.GetPatientsAsync();

            _logger.LogInformation("Retornando todos os Pacientes Registrados");
            var patientsDTO = _mapper.Map<IEnumerable<PatientDTO>>(patients);
            return Ok(patientsDTO);
        }

        /// <summary>
        /// Obtem um Paciente Pelo seu identificador Id.
        /// </summary>
        /// <param name="id">Identificador do Paciente</param>
        /// <returns>Retorna um 200 OK ou 404 NotFound e um PacienteDTO.</returns>
        [HttpGet("{id:int}", Name = "ObterPatient")]
        [Authorize(Policy = "PatientOrAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDTO>> Get(int id)
        {
            _logger.LogInformation($"Buscando o paciente com Id = {id}");
            var patient = await _service.GetPatientAsync(id);

            if (patient is null)
            {
                _logger.LogWarning($"Paciente com o id igual a {id} não encontrado");
                return NotFound($"Paciente com o id igual a {id} não encontrado");
            }

            _logger.LogInformation($"Retornando Paciente do id = {id}");
            var patientDTO = _mapper.Map<PatientDTO>(patient);
            return Ok(patientDTO);
        }

        /// <summary>
        /// Cria um novo Paciente.
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST api/Patient
        ///     {
        ///         "Name": maria,
        ///         "email": maria@gmail.com,
        ///         "Cpf":12345678901
        ///     }
        /// </remarks>
        /// <param name="patientCreateDTO">Objeto PacienteDTO</param>
        /// <returns>Retorna 2001 Created e um PacienteDTO criando.</returns>
        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<PatientDTO>> Post(PatientCreateDTO patientCreateDTO)
        {
            var patient = _mapper.Map<Patient>(patientCreateDTO);

            _logger.LogInformation("Criando um novo paciente");
            var newPatient = await _service.CreateAsync(patient);

            _logger.LogInformation("retornando o novo paciente");
            var newPatientDTO = _mapper.Map<PatientDTO>(newPatient);
            return new CreatedAtRouteResult("ObterPatient", new { id = newPatientDTO.Id }, newPatientDTO);
        }

        /// <summary>
        /// Atualiza um Paciente pelo seu identificador Id.
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     PUT api/Patient
        ///     {
        ///         "Id":1,
        ///         "Name": maria,
        ///         "email": maria@gmail.com,
        ///         "Cpf":12345678901
        ///     }
        /// </remarks>
        /// <param name="id">Identificador do Paciente</param>
        /// <param name="patientUpdateDTO">Objeto PacienteDTO</param>
        /// <returns>Retornar um 200 OK ou 400 BadRequest ou 404 NotFound e um pacienteDTO atualizado.</returns>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "PatientOrAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDTO>> Put(int id, PatientUpdateDTO patientUpdateDTO)
        {
            if (id != patientUpdateDTO.Id)
            {
                _logger.LogInformation("Dados para buscar um paciente estão inválidos");
                return BadRequest("Dados para buscar um paciente estão inválidos");
            }
                
            var existingPatient = await _service.GetPatientAsync(id);
            if (existingPatient is null)
                return NotFound($"Paciente com ID igual a {id} não encontrado.");

            _mapper.Map(patientUpdateDTO, existingPatient);

            _logger.LogInformation("atualizando o paciente");
            var updatingPatient = await _service.UpdateAsync(existingPatient);

            _logger.LogInformation("Retornando o paciente com os dados atualizados");
            var updatingPatientDTO = _mapper.Map<PatientDTO>(updatingPatient);
            return Ok(updatingPatientDTO);
        }

        /// <summary>
        /// Deleta um Paciente pelo seu identificador Id.
        /// </summary>
        /// <param name="id">Identificador do Paciente</param>
        /// <returns>Retorna 204 NoContent ou 404 NotFound</returns>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("Obtendo id do paciente para deletá-lo");
            var patient = await _service.GetPatientAsync(id);

            if (patient is null)
            {
                _logger.LogWarning($"Paciente com id igual a {id} não encontrado");
                return NotFound($"Paciente com o id igual a {id} não encontrado");
            }

            _logger.LogInformation("Removendo o paciente do banco de dados e mostrando as informações do paciente removido");
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
