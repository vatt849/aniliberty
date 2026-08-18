using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Common
{
    public class Image
    {
        [JsonPropertyName("preview")]
        public string Preview { get; set; }
        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }
        [JsonPropertyName("optimized")]
        public OptimizedImage Optimized { get; set; }
    }
}
