using API_SGHSS.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace API_SGHSS.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public DateTime ConsultationDate { get; set; }

        [Required]
        public AppointmentStatus QueryStatus { get; set; }

        public int PatientId { get; set; }

        public Patient? Patient { get; set; }

        public int DoctorId { get; set; }

        public Doctor? Doctor { get; set; }
    }
}
