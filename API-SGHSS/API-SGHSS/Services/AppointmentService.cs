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

        public IEnumerable<Appointment> GetAppointments()
        {
            return _repository.GetAppointments();
        }

        public Appointment GetAppointment(int id)
        {
            return _repository.GetAppointment(id);
        }

        public Appointment Create(Appointment Appointment)
        {
            ValidateAppoitment(Appointment);

            return _repository.Create(Appointment);
        }

        public Appointment Update(Appointment Appointment)
        {
            ValidateAppoitment(Appointment);

            return _repository.Update(Appointment);
        }

        public Appointment Delete(int id)
        {
            var existingAppointment = _repository.GetAppointment(id);

            if (existingAppointment is null)
                throw new ArgumentNullException(nameof(existingAppointment));

            return _repository.Delete(id);
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
