using System.Text.Json.Serialization;

namespace aniliberty.Api.Requests;

internal class LoginRequest
{
    [JsonPropertyName("login")]
    public string Login { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
}
