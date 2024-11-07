namespace MovieWebApp.DTO
{
    public class CustomerRequestDTO
    {
        public int Id { get; set; }

        public string? Address { get; set; }

        public string Gender { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
