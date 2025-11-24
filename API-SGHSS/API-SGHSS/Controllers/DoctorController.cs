using API_SGHSS.DTOs.DoctorDTOs;
using API_SGHSS.Models;
using API_SGHSS.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_SGHSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _repository;
        private readonly ILogger<DoctorController> _logger;
        private readonly IMapper _mapper;

        public DoctorController(IDoctorRepository repository, ILogger<DoctorController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DoctorDTO>> GetAll()
        {
            _logger.LogInformation("Buscando todos os médicos Registrados");
            var doctors = _repository.GetDoctors();

            _logger.LogInformation("Retornando todos os médicos Registrados");
            var doctorsDTO = _mapper.Map<IEnumerable<DoctorDTO>>(doctors);
            return Ok(doctorsDTO);
        }

        [HttpGet("{id:int}", Name ="ObterDoctor")]
        public ActionResult<DoctorDTO> Get(int id)
        {
            _logger.LogInformation($"Buscando o médico com Id = {id}");
            var doctor = _repository.GetDoctor(id);

            if (doctor is null)
            {
                _logger.LogWarning($"Médico com o id igual a {id} não encontrado");
                return NotFound($"Médico com o id igual a {id} não encontrado");
            }

            _logger.LogInformation($"Retornando o médico do id = {id}");
            var doctorDTO = _mapper.Map<DoctorDTO>(doctor);
            return Ok(doctorDTO);
        }

        [HttpPost]
        public ActionResult<DoctorDTO> Post(DoctorCreateDTO doctorCreateDTO)
        {
            if (doctorCreateDTO is null)
            {
                _logger.LogWarning("Dados para cadastrar um novo médico estão inválidos");
                return BadRequest("Dados ao cadastrar um novo médico estão inválidos");
            }

            var doctor = _mapper.Map<Doctor>(doctorCreateDTO);

            _logger.LogInformation("Criando um novo médico");
            var newDoctor = _repository.Create(doctor);

            var newDoctorDTO = _mapper.Map<DoctorDTO>(newDoctor);

            _logger.LogInformation("retornando uma novo médico");
            return new CreatedAtRouteResult("ObterDoctor", new { id = newDoctorDTO.Id }, newDoctorDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<DoctorDTO> Put(int id, DoctorUpdateDTO doctorUpdateDTO)
        {
            if(id != doctorUpdateDTO.Id)
            {
                _logger.LogWarning("Dados para buscar um médico estão inválidos");
                return BadRequest("Dados para buscar um médico estão inválidos");
            }
            
            var existingDoctor = _repository.GetDoctor(id);
            if(existingDoctor is null)
                return NotFound($"Médico com ID igual a {id} não encontrado.");

            _mapper.Map(doctorUpdateDTO, existingDoctor);

            _logger.LogInformation($"atualizando o médico do id = {id}");
            var updatingDoctor = _repository.Update(existingDoctor);

            var updatingDoctorDTO = _mapper.Map<DoctorDTO>(updatingDoctor);

            _logger.LogInformation($"Retornando o médico com os dados atualizados");
            return Ok(updatingDoctorDTO);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation("Obtendo id do médico para deletá-lo");
            var doctor = _repository.GetDoctor(id);

            if (doctor is null)
            {
                _logger.LogWarning($"Médico com id igual a {id} não encontrado");
                return NotFound($"Médico com o id igual a {id} não encontrado");
            }

            _logger.LogInformation("Removendo o médico do banco de dados e mostrando as informações da consulta removido");
            _repository.Delete(id);
            return NoContent();
        }
    }
}
