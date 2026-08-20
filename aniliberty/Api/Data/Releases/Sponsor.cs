using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases
{
    public class Sponsor
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("url_title")]
        public string UrlTitle { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
