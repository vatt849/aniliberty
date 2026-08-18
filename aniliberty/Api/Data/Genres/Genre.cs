using aniliberty.Api.Data.Common;
using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Genres
{
    public class Genre
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("total_releases")]
        public int TotalReleases { get; set; }
        [JsonPropertyName("image")]
        public Image Image { get; set; }
    }
}
