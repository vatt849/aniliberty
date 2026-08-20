using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases
{
    public class AgeRating
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
        [JsonPropertyName("label")]
        public string Label { get; set; }
        [JsonPropertyName("is_adult")]
        public bool IsAdult { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
