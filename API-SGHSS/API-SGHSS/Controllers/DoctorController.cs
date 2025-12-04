using API_SGHSS.DTOs.DoctorDTOs;
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
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _service;
        private readonly ILogger<DoctorController> _logger;
        private readonly IMapper _mapper;

        public DoctorController(IDoctorService service, ILogger<DoctorController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtem uma lista de Doutores.
        /// </summary>
        /// <returns>Retorna 200 OK e uma lista de DoutorDTO</returns>
        [HttpGet]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetAll()
        {
            _logger.LogInformation("Buscando todos os médicos Registrados");
            var doctors = await _service.GetDoctorsAsync();

            _logger.LogInformation("Retornando todos os médicos Registrados");
            var doctorsDTO = _mapper.Map<IEnumerable<DoctorDTO>>(doctors);
            return Ok(doctorsDTO);
        }

        /// <summary>
        /// Obtem um Doutor pelo seu identificador Id.
        /// </summary>
        /// <param name="id">Identificador do doutor</param>
        /// <returns>Retorna um 200 OK ou 404 NotFound e um DoutorDTO</returns>
        [HttpGet("{id:int}", Name ="ObterDoctor")]
        [Authorize(Policy = "DoctorOrAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DoctorDTO>> Get(int id)
        {
            _logger.LogInformation($"Buscando o médico com Id = {id}");
            var doctor = await _service.GetDoctorAsync(id);

            if (doctor is null)
            {
                _logger.LogWarning($"Médico com o id igual a {id} não encontrado");
                return NotFound($"Médico com o id igual a {id} não encontrado");
            }

            _logger.LogInformation($"Retornando o médico do id = {id}");
            var doctorDTO = _mapper.Map<DoctorDTO>(doctor);
            return Ok(doctorDTO);
        }

        /// <summary>
        /// Cria um novo Doutor.
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST api/Doctor
        ///         {
        ///             "Name": carlos,
        ///             "email": carlos@gmail.com,
        ///             "Crm":123456789
        ///         }
        /// </remarks>
        /// <param name="doctorCreateDTO">Objeto Doutor.</param>
        /// <returns>Retorna 201 Created e um DoutorDTO criando.</returns>
        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<DoctorDTO>> Post(DoctorCreateDTO doctorCreateDTO)
        {
            var doctor = _mapper.Map<Doctor>(doctorCreateDTO);

            _logger.LogInformation("Criando um novo médico");
            var newDoctor = await _service.CreateAsync(doctor);

            _logger.LogInformation("retornando uma novo médico");
            var newDoctorDTO = _mapper.Map<DoctorDTO>(newDoctor);
            return new CreatedAtRouteResult("ObterDoctor", new { id = newDoctorDTO.Id }, newDoctorDTO);
        }

        /// <summary>
        /// Atualiza um Doutor pelo seu identificador Id.
        /// </summary>
        /// <remarks>
        ///     PUT api/Doctor
        ///         {
        ///             "Id":1,
        ///             "Name": carlos,
        ///             "email": carlos@gmail.com,
        ///             "Crm":987654321
        ///         }
        /// </remarks>
        /// <param name="id">Identificador do Doutor</param>
        /// <param name="doctorUpdateDTO">Objeto doutor</param>
        /// <returns>Retornar um 200 OK ou 400 BadRequest ou 404 NotFound e um doutorDTO atualizado.</returns>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "DoctorOrAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DoctorDTO>> Put(int id, DoctorUpdateDTO doctorUpdateDTO)
        {
            if(id != doctorUpdateDTO.Id)
            {
                _logger.LogWarning("Dados para buscar um médico estão inválidos");
                return BadRequest("Dados para buscar um médico estão inválidos");
            }
            
            var existingDoctor = await _service.GetDoctorAsync(id);
            if(existingDoctor is null)
                return NotFound($"Médico com ID igual a {id} não encontrado.");

            _mapper.Map(doctorUpdateDTO, existingDoctor);

            _logger.LogInformation($"atualizando o médico do id = {id}");
            var updatingDoctor = await _service.UpdateAsync(existingDoctor);

            _logger.LogInformation($"Retornando o médico com os dados atualizados");
            var updatingDoctorDTO = _mapper.Map<DoctorDTO>(updatingDoctor);
            return Ok(updatingDoctorDTO);
        }

        /// <summary>
        /// Deleta um Doutor pelo seu identificador Id.
        /// </summary>
        /// <param name="id">Identificador do Doutor.</param>
        /// <returns>Retorna um 204 NoContent ou 404 NotFound</returns>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("Obtendo id do médico para deletá-lo");
            var doctor = await _service.GetDoctorAsync(id);

            if (doctor is null)
            {
                _logger.LogWarning($"Médico com id igual a {id} não encontrado");
                return NotFound($"Médico com o id igual a {id} não encontrado");
            }

            _logger.LogInformation("Removendo o médico do banco de dados e mostrando as informações da consulta removido");
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}