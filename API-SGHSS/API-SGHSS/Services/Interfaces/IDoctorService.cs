using API_SGHSS.Models;

namespace API_SGHSS.Services.Interfaces
{
    public interface IDoctorService
    {
        public IEnumerable<Doctor> GetDoctors();
        public Doctor GetDoctor(int id);
        public Doctor Create(Doctor doctor);
        public Doctor Update(Doctor doctor);
        public Doctor Delete(int id);
    }
}
