using API_SGHSS.Models;

namespace API_SGHSS.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        public IEnumerable<Appointment> GetAppointments();
        public Appointment GetAppointment(int id);
        public Appointment Create(Appointment appointment);
        public Appointment Update(Appointment appointment);
        public Appointment Delete(int id);
    }
}
