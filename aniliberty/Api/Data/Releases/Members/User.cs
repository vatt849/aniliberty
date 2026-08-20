using aniliberty.Api.Data.Common;
using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases.Members
{
    public class User
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }
        [JsonPropertyName("avatar")]
        public Image Avatar { get; set; }
    }
}
