using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases.Members
{
    public class Role
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
