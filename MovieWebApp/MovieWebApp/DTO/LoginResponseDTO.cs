namespace MovieWebApp.DTO
{
    public class LoginResponseDTO
    {
        public AccountDTO? account { get; set; } 
        public string token { get; set; } = string.Empty;
    }
}
