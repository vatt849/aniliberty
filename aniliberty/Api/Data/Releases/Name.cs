using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases
{
    public class Name
    {
        [JsonPropertyName("main")]
        public string Main { get; set; }
        [JsonPropertyName("english")]
        public string English { get; set; }
        [JsonPropertyName("alternative")]
        public string Alternative { get; set; }
    }
}
