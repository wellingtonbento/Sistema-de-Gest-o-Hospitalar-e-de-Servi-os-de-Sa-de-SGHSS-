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

        public IEnumerable<Patient> GetPatients()
        {
            return _context.Patients.ToList();
        }

        public Patient GetPatient(int id)
        {
            return _context.Patients.FirstOrDefault(p => p.Id == id);
        }

        public Patient Create(Patient patient)
        {
            if (patient is null)
                throw new ArgumentNullException(nameof(patient));

            _context.Patients.Add(patient);
            _context.SaveChanges();
            return patient;
        }

        public Patient Update(Patient patient)
        {
            if (patient is null)
                throw new ArgumentNullException(nameof(patient));

            _context.Entry(patient).State = EntityState.Modified;
            _context.SaveChanges();
            return patient;
        }

        public Patient Delete(int id)
        {
            var patient = _context.Patients.Find(id);

            if (patient is null)
                throw new ArgumentNullException(nameof(patient));

            _context.Remove(patient);
            _context.SaveChanges();
            return patient;
        }
    }
}
