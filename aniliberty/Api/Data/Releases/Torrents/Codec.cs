using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases.Torrents
{
    public class Codec
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
