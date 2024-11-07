namespace MovieWebApp.DTO
{
    public class ResponseDTO
    {
        public object? result { get; set; }
        public bool isSuccess { get; set; } = true;
        public string message { get; set; } = string.Empty;
    }
}
