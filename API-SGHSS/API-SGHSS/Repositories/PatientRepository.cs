using API_SGHSS.Context;
using API_SGHSS.Models;
using API_SGHSS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_SGHSS.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly SGHSSContext _context;

        public PatientRepository(SGHSSContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetPatientsAsync()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patient> GetPatientAsync(int id)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Patient> CreateAsync(Patient patient)
        {
            if (patient is null)
                throw new ArgumentNullException(nameof(patient));

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<Patient> UpdateAsync(Patient patient)
        {
            if (patient is null)
                throw new ArgumentNullException(nameof(patient));

            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<Patient> DeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient is null)
                throw new ArgumentNullException(nameof(patient));

            _context.Remove(patient);
            await _context.SaveChangesAsync();
            return patient;
        }
    }
}
