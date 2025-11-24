using API_SGHSS.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace API_SGHSS.DTOs.AppointmentDTOs
{
    public class AppointmentUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public DateTime ConsultationDate { get; set; }

        [Required]
        public AppointmentStatus QueryStatus { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}
