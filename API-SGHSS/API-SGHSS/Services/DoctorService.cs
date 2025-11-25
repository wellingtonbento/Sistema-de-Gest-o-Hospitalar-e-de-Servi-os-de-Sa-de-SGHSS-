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

        public IEnumerable<Doctor> GetDoctors()
        {
            return _repository.GetDoctors();
        }

        public Doctor GetDoctor(int id)
        {
            return _repository.GetDoctor(id);
        }

        public Doctor Create(Doctor doctor)
        {
            ValidateDoctor(doctor);
            
            return _repository.Create(doctor);
        }

        public Doctor Update(Doctor doctor)
        {
            ValidateDoctor(doctor);

            return _repository.Update(doctor);
        }

        public Doctor Delete(int id)
        {
            var existingDoctor = _repository.GetDoctor(id);

            if (existingDoctor is null)
                throw new ArgumentNullException(nameof(existingDoctor));

            return _repository.Delete(id);
        }

        private void ValidateDoctor(Doctor doctor)
        {
            if (doctor is null)
                throw new ArgumentNullException(nameof(doctor));

            if (doctor.Name.Length < 3 || doctor.Name.Length > 100)
                throw new ArgumentException("O nome deve conter entre 3 e 100 caracteres.");

            if (doctor.Crm.Length != 9)
                throw new ArgumentException("O CPF deve conter exatamente 9 dígitos.");
        }
    }
}
