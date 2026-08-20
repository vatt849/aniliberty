using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases.Episodes
{
    public class Interval
    {
        [JsonPropertyName("start")]
        public int? Start { get; set; }
        [JsonPropertyName("stop")]
        public int? Stop { get; set; }
    }
}
