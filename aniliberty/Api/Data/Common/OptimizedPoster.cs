using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Common
{
    public class OptimizedPoster
    {
        [JsonPropertyName("src")]
        public string Src { get; set; }
        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }
    }
}
