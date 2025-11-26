using API_SGHSS.Models;

namespace API_SGHSS.Services.Interfaces
{
    public interface IAppointmentService
    {
        public Task<IEnumerable<Appointment>> GetAppointmentsAsync();
        public Task<Appointment> GetAppointmentAsync(int id);
        public Task<Appointment> CreateAsync(Appointment Appointment);
        public Task<Appointment> UpdateAsync(Appointment Appointment);
        public Task<Appointment> DeleteAsync(int id);
    }
}
