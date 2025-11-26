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

        public async Task<IEnumerable<Patient>> GetPatientsAsync()
        {
            return await _repository.GetPatientsAsync();
        }

        public async Task<Patient> GetPatientAsync(int id)
        {
            return await _repository.GetPatientAsync(id);
        }

        public async Task<Patient> CreateAsync(Patient patient)
        {
            ValidatePatient(patient);

            return await _repository.CreateAsync(patient);
        }

        public async Task<Patient> UpdateAsync(Patient patient)
        {
            ValidatePatient(patient);

            var existingPatient = await _repository.GetPatientAsync(patient.Id);

            if (existingPatient is null)
                throw new ArgumentNullException(nameof(existingPatient));

            return await _repository.UpdateAsync(patient);
        }

        public async Task<Patient> DeleteAsync(int id)
        {
            var existingPatient = await _repository.GetPatientAsync(id);

            if(existingPatient is null)
                throw new ArgumentNullException(nameof(existingPatient));

            return await _repository.DeleteAsync(id);
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
