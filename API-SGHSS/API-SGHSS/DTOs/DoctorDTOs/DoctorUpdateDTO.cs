using System.ComponentModel.DataAnnotations;

namespace API_SGHSS.DTOs.DoctorDTOs
{
    public class DoctorUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Crm { get; set; }
    }
}
