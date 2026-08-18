using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Common
{
    public class Poster
    {
        [JsonPropertyName("src")]
        public string Src { get; set; }
        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }
        [JsonPropertyName("optimized")]
        public OptimizedPoster Optimized { get; set; }
    }
}
