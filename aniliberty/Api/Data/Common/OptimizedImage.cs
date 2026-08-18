using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Common
{
    public class OptimizedImage
    {
        [JsonPropertyName("preview")]
        public string Preview { get; set; }
        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }
    }
}
