using System.ComponentModel.DataAnnotations;

namespace API_SGHSS.DTOs.AuthDTOs
{
    public class LoginDTO
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
