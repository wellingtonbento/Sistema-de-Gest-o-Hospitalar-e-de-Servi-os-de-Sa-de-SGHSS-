using API_SGHSS.Models;

namespace API_SGHSS.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        public IEnumerable<Patient> GetPatients();
        public Patient GetPatient(int id);
        public Patient Create(Patient patient);
        public Patient Update(Patient patient);
        public Patient Delete(int id);
    }
}
