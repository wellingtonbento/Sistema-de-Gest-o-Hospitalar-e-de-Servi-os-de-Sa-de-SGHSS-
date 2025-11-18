using System.Collections.ObjectModel;

namespace API_SGHSS.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Crm { get; set; }
        public IEnumerable<Appointment>? Appointments { get; set; }

        public Doctor()
        {
            Appointments = new Collection<Appointment>();
        }
    }
}

