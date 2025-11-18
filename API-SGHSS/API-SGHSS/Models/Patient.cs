using System.Collections.ObjectModel;

namespace API_SGHSS.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Cpf { get; set; }
        public IEnumerable<Appointment>? Appointments { get; set; }

        public Patient()
        {
            Appointments = new Collection<Appointment>();
        }
    }
}
