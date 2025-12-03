using API_SGHSS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API_SGHSS.Context
{
    public class SGHSSContext : IdentityDbContext<User>
    {
        public SGHSSContext(DbContextOptions<SGHSSContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            //Patient
            mb.Entity<Patient>().HasKey(k => k.Id);
            mb.Entity<Patient>().Property(n => n.Name).HasMaxLength(100).IsRequired();
            mb.Entity<Patient>().Property(e => e.Email).HasMaxLength(150).IsRequired();
            mb.Entity<Patient>().Property(c => c.Cpf).HasMaxLength(11).IsRequired();

            //Doctor
            mb.Entity<Doctor>().HasKey(k => k.Id);
            mb.Entity<Doctor>().Property(n => n.Name).HasMaxLength(100).IsRequired();
            mb.Entity<Doctor>().Property(e => e.Email).HasMaxLength(150).IsRequired();
            mb.Entity<Doctor>().Property(c => c.Crm).HasMaxLength(9).IsRequired();

            //Appointment
            mb.Entity<Appointment>().HasKey(k => k.Id);
            mb.Entity<Appointment>().Property(d => d.Description).HasMaxLength(250).IsRequired();
            mb.Entity<Appointment>().Property(q => q.QueryStatus).HasConversion<string>().HasMaxLength(20).IsRequired();

            //relationship
            mb.Entity<Appointment>()
                .HasOne<Patient>(p =>p.Patient)
                .WithMany(a => a.Appointments).HasForeignKey(p => p.PatientId);

            mb.Entity<Appointment>()
               .HasOne<Doctor>(d => d.Doctor)
               .WithMany(a => a.Appointments).HasForeignKey(d => d.DoctorId);

        }
    }
}
