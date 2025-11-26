using API_SGHSS.Context;
using API_SGHSS.Models;
using API_SGHSS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_SGHSS.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly SGHSSContext _context;

        public AppointmentRepository(SGHSSContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<Appointment> GetAppointmentAsync(int id)
        {
            return await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Appointment> CreateAsync(Appointment appointment)
        {
            if(appointment is null)
                throw new ArgumentNullException(nameof(appointment));

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment> UpdateAsync(Appointment appointment)
        {
            if (appointment is null)
                throw new ArgumentNullException(nameof(appointment));

            _context.Entry(appointment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment> DeleteAsync(int id)
        {
            var doctor = _context.Appointments.Find(id);

            if (doctor is null)
                throw new ArgumentNullException(nameof(doctor));

            _context.Remove(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }
    }
}
