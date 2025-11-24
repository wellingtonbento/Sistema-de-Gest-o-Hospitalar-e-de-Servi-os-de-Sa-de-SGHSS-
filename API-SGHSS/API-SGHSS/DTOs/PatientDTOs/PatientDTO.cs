using System.ComponentModel.DataAnnotations;

namespace API_SGHSS.DTOs.PatientDTOs
{
    public class PatientDTO
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Cpf { get; set; }
    }
}
