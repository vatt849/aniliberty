using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases
{
    public class PublishDay
    {
        [JsonPropertyName("value")]
        public int Value { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
