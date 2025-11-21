using API_SGHSS.Context;
using API_SGHSS.Models;
using API_SGHSS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_SGHSS.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly SGHSSContext _context;

        public DoctorRepository(SGHSSContext context)
        {
            _context = context;
        }

        public IEnumerable<Doctor> GetDoctors()
        {
            return _context.Doctors.ToList();
        }

        public Doctor GetDoctor(int id)
        {
            return _context.Doctors.FirstOrDefault(d => d.Id == id);
        }

        public Doctor Create(Doctor doctor)
        {
            if (doctor is null)
                throw new ArgumentNullException(nameof(doctor));

            _context.Doctors.Add(doctor);
            _context.SaveChanges();
            return doctor;
        }

        public Doctor Update(Doctor doctor)
        {
            if (doctor is null)
                throw new ArgumentNullException(nameof(doctor));

            _context.Entry(doctor).State = EntityState.Modified;
            _context.SaveChanges();
            return doctor;
        }

        public Doctor Delete(int id)
        {
            var doctor = _context.Doctors.Find(id);

            if (doctor is null)
                throw new ArgumentNullException(nameof(doctor));

            _context.Remove(doctor);
            _context.SaveChanges();
            return doctor;
        }
    }
}
