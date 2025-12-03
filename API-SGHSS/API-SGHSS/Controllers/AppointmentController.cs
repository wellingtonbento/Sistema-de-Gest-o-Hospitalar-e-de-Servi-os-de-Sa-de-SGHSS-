using API_SGHSS.DTOs.AppointmentDTOs;
using API_SGHSS.Models;
using API_SGHSS.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_SGHSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;
        private readonly ILogger<AppointmentController> _logger;
        private readonly IMapper _mapper;

        public AppointmentController(IAppointmentService service, ILogger<AppointmentController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAll()
        {
            _logger.LogInformation("Buscando todas as consultas Registrados");
            var appointments = await _service.GetAppointmentsAsync();

            _logger.LogInformation("Retornando todas as consultas Registrados");
            var appointmentsDTO = _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
            return Ok(appointmentsDTO);
        }

        [HttpGet("{id:int}", Name = "ObterAppointment")]
        [Authorize]
        public async Task<ActionResult<AppointmentDTO>> Get(int id)
        {
            _logger.LogInformation($"Buscando a consutla com Id = {id}");
            var appointment = await _service.GetAppointmentAsync(id);

            if (appointment is null)
            {
                _logger.LogWarning($"Consulta com o id igual a {id} não encontrado");
                return NotFound($"Consulta com o id igual a {id} não encontrado");
            }

            _logger.LogInformation($"Retornando a consulta do id = {id}");
            var appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);
            return Ok(appointmentDTO);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AppointmentDTO>> Post(AppointmentCreateDTO appointmentCreateDTO)
        {
            var appointment = _mapper.Map<Appointment>(appointmentCreateDTO);

            _logger.LogInformation("Criando uma nova consulta");
            var newAppointment = await _service.CreateAsync(appointment);

            _logger.LogInformation("retornando uma nova consulta");
            var newAppointmentDTO = _mapper.Map<AppointmentDTO>(newAppointment);
            return new CreatedAtRouteResult("ObterAppointment", new { id = newAppointmentDTO.Id }, newAppointmentDTO);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<ActionResult<AppointmentDTO>> Put(int id, AppointmentUpdateDTO appointmentUpdateDTO)
        {
            if (id != appointmentUpdateDTO.Id)
            {
                _logger.LogWarning("Os IDs da rota e do corpo precisam ser iguais.");
                return BadRequest("Os IDs da rota e do corpo precisam ser iguais.");
            }

            var existingAppointment = await _service.GetAppointmentAsync(id);
            if(existingAppointment is null)
                return NotFound($"Consulta com ID igual a {id} não encontrado.");

            _mapper.Map(appointmentUpdateDTO, existingAppointment);

            _logger.LogInformation($"atualizando a consulta do id = {id}");
            var updatingAppointment = await _service.UpdateAsync(existingAppointment);

            _logger.LogInformation($"Retornando a consulta com os dados atualizados");
            var updatingAppointmentDTO = _mapper.Map<AppointmentDTO>(updatingAppointment);
            return Ok(updatingAppointmentDTO);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("Obtendo id da consulta para deletá-lo");
            var appointment = await _service.GetAppointmentAsync(id);

            if (appointment is null)
            {
                _logger.LogWarning($"Consulta com id igual a {id} não encontrado");
                return NotFound($"Consulta com o id igual a {id} não encontrado");
            }

            _logger.LogInformation("Removendo a consulta do banco de dados e mostrando as informações da consulta removido");
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
