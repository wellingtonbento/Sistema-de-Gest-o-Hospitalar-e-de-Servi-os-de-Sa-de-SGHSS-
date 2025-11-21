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

        public IEnumerable<Appointment> GetAppointments()
        {
            return _context.Appointments.ToList();
        }

        public Appointment GetAppointment(int id)
        {
            return _context.Appointments.FirstOrDefault(a => a.Id == id);
        }

        public Appointment Create(Appointment appointment)
        {
            if(appointment is null)
                throw new ArgumentNullException(nameof(appointment));

            _context.Appointments.Add(appointment);
            _context.SaveChanges();
            return appointment;
        }

        public Appointment Update(Appointment appointment)
        {
            if (appointment is null)
                throw new ArgumentNullException(nameof(appointment));

            _context.Entry(appointment).State = EntityState.Modified;
            _context.SaveChanges();
            return appointment;
        }

        public Appointment Delete(int id)
        {
            var doctor = _context.Appointments.Find(id);

            if (doctor is null)
                throw new ArgumentNullException(nameof(doctor));

            _context.Remove(doctor);
            _context.SaveChanges();
            return doctor;
        }
    }
}
