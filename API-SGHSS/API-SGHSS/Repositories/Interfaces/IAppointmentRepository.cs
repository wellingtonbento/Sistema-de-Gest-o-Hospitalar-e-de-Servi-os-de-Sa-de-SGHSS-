using API_SGHSS.Models;

namespace API_SGHSS.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        public Task<IEnumerable<Appointment>> GetAppointmentsAsync();
        public Task<Appointment> GetAppointmentAsync(int id);
        public Task<Appointment> CreateAsync(Appointment appointment);
        public Task<Appointment> UpdateAsync(Appointment appointment);
        public Task<Appointment> DeleteAsync(int id);
    }
}
