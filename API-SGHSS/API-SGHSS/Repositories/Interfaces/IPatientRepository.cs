using API_SGHSS.Models;

namespace API_SGHSS.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        public Task<IEnumerable<Patient>> GetPatientsAsync();
        public Task<Patient> GetPatientAsync(int id);
        public Task<Patient> CreateAsync(Patient patient);
        public Task<Patient> UpdateAsync(Patient patient);
        public Task<Patient> DeleteAsync(int id);
    }
}
