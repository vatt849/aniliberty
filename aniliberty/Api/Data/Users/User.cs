using aniliberty.Api.Data.Common;
using System;
using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Users
{
    public class User
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("login")]
        public string Login { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }
        [JsonPropertyName("avatar")]
        public Image Avatar { get; set; }
        [JsonPropertyName("torrents")]
        public TorrentStats Torrents { get; set; }
        [JsonPropertyName("is_banned")]
        public bool IsBanned { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("is_with_ads")]
        public bool IsWithAds { get; set; }

        public string AvatarUrl { get => Static.ToFullUrl(Avatar.Preview); }
        public string ThumbnailUrl { get => Static.ToFullUrl(Avatar.Thumbnail); }
    }
}
