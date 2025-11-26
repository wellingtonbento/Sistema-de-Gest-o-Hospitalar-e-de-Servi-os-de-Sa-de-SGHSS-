using API_SGHSS.Models;

namespace API_SGHSS.Services.Interfaces
{
    public interface IDoctorService
    {
        public Task<IEnumerable<Doctor>> GetDoctorsAsync();
        public Task<Doctor> GetDoctorAsync(int id);
        public Task<Doctor> CreateAsync(Doctor doctor);
        public Task<Doctor> UpdateAsync(Doctor doctor);
        public Task<Doctor> DeleteAsync(int id);
    }
}
