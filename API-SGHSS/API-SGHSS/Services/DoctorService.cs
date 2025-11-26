using API_SGHSS.Models;
using API_SGHSS.Repositories.Interfaces;
using API_SGHSS.Services.Interfaces;

namespace API_SGHSS.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;

        public DoctorService(IDoctorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
        {
            return await _repository.GetDoctorsAsync();
        }

        public async Task<Doctor> GetDoctorAsync(int id)
        {
            return await _repository.GetDoctorAsync(id);
        }

        public async Task<Doctor> CreateAsync(Doctor doctor)
        {
            ValidateDoctor(doctor);
            
            return await _repository.CreateAsync(doctor);
        }

        public async Task<Doctor> UpdateAsync(Doctor doctor)
        {
            ValidateDoctor(doctor);

            return await _repository.UpdateAsync(doctor);
        }

        public async Task<Doctor> DeleteAsync(int id)
        {
            var existingDoctor = await _repository.GetDoctorAsync(id);

            if (existingDoctor is null)
                throw new ArgumentNullException(nameof(existingDoctor));

            return await _repository.DeleteAsync(id);
        }

        private void ValidateDoctor(Doctor doctor)
        {
            if (doctor is null)
                throw new ArgumentNullException(nameof(doctor));

            if (doctor.Name.Length < 3 || doctor.Name.Length > 100)
                throw new ArgumentException("O nome deve conter entre 3 e 100 caracteres.");

            if (doctor.Crm.Length != 9)
                throw new ArgumentException("O CRM deve conter exatamente 9 dígitos.");
        }
    }
}
