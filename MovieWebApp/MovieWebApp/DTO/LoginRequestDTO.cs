using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
