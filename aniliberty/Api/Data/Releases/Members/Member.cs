using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases.Members
{
    public class Member
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        [JsonPropertyName("user")]
        public User User { get; set; }
        [JsonPropertyName("role")]
        public Role Role { get; set; }
        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }
    }
}
