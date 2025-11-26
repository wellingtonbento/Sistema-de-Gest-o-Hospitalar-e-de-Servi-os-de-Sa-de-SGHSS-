using API_SGHSS.Models;
using API_SGHSS.Repositories.Interfaces;
using API_SGHSS.Services.Interfaces;

namespace API_SGHSS.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsAsync()
        {
            return await _repository.GetAppointmentsAsync();
        }

        public async Task<Appointment> GetAppointmentAsync(int id)
        {
            return await _repository.GetAppointmentAsync(id);
        }

        public async Task<Appointment> CreateAsync(Appointment Appointment)
        {
            ValidateAppoitment(Appointment);

            return await _repository.CreateAsync(Appointment);
        }

        public async Task<Appointment> UpdateAsync(Appointment Appointment)
        {
            ValidateAppoitment(Appointment);

            return await _repository.UpdateAsync(Appointment);
        }

        public async Task<Appointment> DeleteAsync(int id)
        {
            var existingAppointment = await _repository.GetAppointmentAsync(id);

            if (existingAppointment is null)
                throw new ArgumentNullException(nameof(existingAppointment));

            return await _repository.DeleteAsync(id);
        }

        private void ValidateAppoitment(Appointment appointment)
        {
            if (appointment is null)
                throw new ArgumentNullException(nameof(appointment));

            if (appointment.Description.Length < 3 || appointment.Description.Length > 250)
                throw new ArgumentException("A descrição deve conter entre 3 e 250 caracteres");

            if (appointment.ConsultationDate.Date == DateTime.Today)
                throw new ArgumentException("consulta não pode ser marcada para hoje");

            if (appointment.ConsultationDate.Date <= DateTime.Today)
                throw new ArgumentException("consulta não pode ser marcada em uma data que já passou");
        }
    }
}
