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

        public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
        {
            return await _context.Doctors.ToListAsync();
        }

        public async Task<Doctor> GetDoctorAsync(int id)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Doctor> CreateAsync(Doctor doctor)
        {
            if (doctor is null)
                throw new ArgumentNullException(nameof(doctor));

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<Doctor> UpdateAsync(Doctor doctor)
        {
            if (doctor is null)
                throw new ArgumentNullException(nameof(doctor));

            _context.Entry(doctor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<Doctor> DeleteAsync(int id)
        {
            var doctor = _context.Doctors.Find(id);

            if (doctor is null)
                throw new ArgumentNullException(nameof(doctor));

            _context.Remove(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }
    }
}
