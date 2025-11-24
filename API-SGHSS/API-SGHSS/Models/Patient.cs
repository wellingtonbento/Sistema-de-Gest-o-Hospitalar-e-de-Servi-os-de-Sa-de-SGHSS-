using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API_SGHSS.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Cpf { get; set; }

        public IEnumerable<Appointment>? Appointments { get; set; }

        public Patient()
        {
            Appointments = new Collection<Appointment>();
        }
    }
}
