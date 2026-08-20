using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases.Torrents
{
    public class Quality
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
