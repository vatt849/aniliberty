using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Users
{
    public class TorrentStats
    {
        [JsonPropertyName("passkey")]
        public string Passkey { get; set; }
        [JsonPropertyName("uploaded")]
        public long? Uploaded { get; set; }
        [JsonPropertyName("downloaded")]
        public long? Downloaded { get; set; }
    }
}
