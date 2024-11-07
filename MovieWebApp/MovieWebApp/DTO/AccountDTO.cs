using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.DTO
{
    public class AccountDTO
    {
        [Required]
        public string Email { get; set; } 

        [Required]
        public string Password { get; set; } 
        public byte[]? Salt { get; set; }

        public bool Active { get; set; }

        public string? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
