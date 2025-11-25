using API_SGHSS.Models;

namespace API_SGHSS.Services.Interfaces
{
    public interface IAppointmentService
    {
        public IEnumerable<Appointment> GetAppointments();
        public Appointment GetAppointment(int id);
        public Appointment Create(Appointment Appointment);
        public Appointment Update(Appointment Appointment);
        public Appointment Delete(int id);
    }
}
