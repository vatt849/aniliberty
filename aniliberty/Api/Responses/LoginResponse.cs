using System.Text.Json.Serialization;

namespace aniliberty.Api.Responses;

public class LoginResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
}
