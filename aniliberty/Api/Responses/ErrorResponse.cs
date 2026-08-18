using System.Text.Json.Serialization;

namespace aniliberty.Api.Responses;

internal class ErrorResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
}
