using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API_SGHSS.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Crm { get; set; }

        [JsonIgnore]
        public IEnumerable<Appointment>? Appointments { get; set; }

        public Doctor()
        {
            Appointments = new Collection<Appointment>();
        }
    }
}

