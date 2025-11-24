using System.ComponentModel.DataAnnotations;

namespace API_SGHSS.DTOs.DoctorDTOs
{
    public class DoctorCreateDTO
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Crm { get; set; }
    }
}
