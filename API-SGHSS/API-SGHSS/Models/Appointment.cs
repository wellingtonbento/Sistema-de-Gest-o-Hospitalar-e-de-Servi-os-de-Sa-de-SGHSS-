using API_SGHSS.Models.Enums;
using System.Text.Json.Serialization;

namespace API_SGHSS.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime ConsultationDate { get; set; }
        public AppointmentStatus QueryStatus { get; set; }

        public int PatientId { get; set; }

        [JsonIgnore]
        public Patient? Patient { get; set; }

        public int DoctorId { get; set; }

        [JsonIgnore]
        public Doctor? Doctor { get; set; }
    }
}
