namespace MovieWebApp.DTO
{
    public class ExternalAuthDTO
    {
        public string IdToken { get; set; } = "";
        public string Provider { get; set; } = string.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty ;
        public string PhotoUrl { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;

    }
}
