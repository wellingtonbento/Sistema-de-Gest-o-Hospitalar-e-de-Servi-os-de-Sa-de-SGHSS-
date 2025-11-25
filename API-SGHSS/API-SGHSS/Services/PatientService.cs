using API_SGHSS.Models;
using API_SGHSS.Repositories.Interfaces;
using API_SGHSS.Services.Interfaces;

namespace API_SGHSS.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Patient> GetPatients()
        {
            return _repository.GetPatients();
        }

        public Patient GetPatient(int id)
        {
            return _repository.GetPatient(id);
        }

        public Patient Create(Patient patient)
        {
            ValidatePatient(patient);

            return _repository.Create(patient);
        }

        public Patient Update(Patient patient)
        {
            ValidatePatient(patient);

            var existingPatient = _repository.GetPatient(patient.Id);

            if (existingPatient is null)
                throw new ArgumentNullException(nameof(existingPatient));

            return _repository.Update(patient);
        }

        public Patient Delete(int id)
        {
            var existingPatient = _repository.GetPatient(id);

            if(existingPatient is null)
                throw new ArgumentNullException(nameof(existingPatient));

            return _repository.Delete(id);
        }

        private void ValidatePatient(Patient patient)
        {
            if (patient is null)
                throw new ArgumentNullException(nameof(patient));

            if (patient.Name.Length < 3 || patient.Name.Length > 100)
                throw new ArgumentException("O nome deve conter entre 3 e 100 caracteres.");

            if (patient.Cpf.Length != 11)
                throw new ArgumentException("O CPF deve conter exatamente 11 dígitos.");
        }
    }
}
