using API_SGHSS.Models;

namespace API_SGHSS.Services.Interfaces
{
    public interface IPatientService
    {
        public Task<IEnumerable<Patient>> GetPatientsAsync();
        public Task<Patient> GetPatientAsync(int id);
        public Task<Patient> CreateAsync(Patient patient);
        public Task<Patient> UpdateAsync(Patient patient);
        public Task<Patient> DeleteAsync(int id);
    }
}
