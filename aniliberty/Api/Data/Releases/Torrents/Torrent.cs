using System;
using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases.Torrents
{
    public class Torrent
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("hash")]
        public string Hash { get; set; }
        [JsonPropertyName("size")]
        public long Size { get; set; }
        [JsonPropertyName("type")]
        public Type Type { get; set; }
        [JsonPropertyName("label")]
        public string Label { get; set; }
        [JsonPropertyName("magnet")]
        public string Magnet { get; set; }
        [JsonPropertyName("filename")]
        public string Filename { get; set; }
        [JsonPropertyName("seeders")]
        public int Seeders { get; set; }
        [JsonPropertyName("quality")]
        public Quality Quality { get; set; }
        [JsonPropertyName("codec")]
        public Codec Codec { get; set; }
        [JsonPropertyName("color")]
        public Color Color { get; set; }
        [JsonPropertyName("bitrate")]
        public int? Bitrate { get; set; }
        [JsonPropertyName("leechers")]
        public int Leechers { get; set; }
        [JsonPropertyName("sort_order")]
        public int SortOrder { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("completed_times")]
        public int CompletedTimes { get; set; }
    }
}
