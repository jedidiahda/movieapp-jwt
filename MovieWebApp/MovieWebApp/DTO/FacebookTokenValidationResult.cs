using System.Text.Json.Serialization;

namespace MovieWebApp.DTO
{
    public class FacebookTokenValidationResult
    {
        [JsonPropertyName("data")] public FacebookTokenValidationData Data { get; set; } = null!;

    }
}
