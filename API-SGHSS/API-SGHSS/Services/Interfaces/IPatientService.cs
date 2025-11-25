using API_SGHSS.Models;

namespace API_SGHSS.Services.Interfaces
{
    public interface IPatientService
    {
        public IEnumerable<Patient> GetPatients();
        public Patient GetPatient(int id);
        public Patient Create(Patient patient);
        public Patient Update(Patient patient);
        public Patient Delete(int id);
    }
}
