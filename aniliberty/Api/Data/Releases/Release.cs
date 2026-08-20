using aniliberty.Api.Data.Common;
using System;
using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases
{
    public class Release
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("type")]
        public Type Type { get; set; }
        [JsonPropertyName("year")]
        public int Year { get; set; }
        [JsonPropertyName("name")]
        public Name Name { get; set; }
        [JsonPropertyName("alias")]
        public string Alias { get; set; }
        [JsonPropertyName("season")]
        public Season Season { get; set; }
        [JsonPropertyName("poster")]
        public Poster Poster { get; set; }
        [JsonPropertyName("fresh_at")]
        public DateTime? FreshAt { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonPropertyName("is_ongoing")]
        public bool IsOngoing { get; set; }
        [JsonPropertyName("age_rating")]
        public AgeRating AgeRating { get; set; }
        [JsonPropertyName("publish_day")]
        public PublishDay PublishDay { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("notification")]
        public string Notification { get; set; }
        [JsonPropertyName("episodes_total")]
        public int? EpisodesTotal { get; set; }
        [JsonPropertyName("external_player")]
        public string ExternalPlayer { get; set; }
        [JsonPropertyName("is_in_production")]
        public bool IsInProduction { get; set; }
        [JsonPropertyName("is_blocked_by_geo")]
        public bool IsBlockedByGeo { get; set; }
        [JsonPropertyName("episodes_are_unknown")]
        public bool IsEpisodesAreUnknown { get; set; }
        [JsonPropertyName("is_blocked_by_copyrights")]
        public bool IsBlockedByCopyrights { get; set; }
        [JsonPropertyName("added_in_users_favorites")]
        public int AddedInUsersFavorites { get; set; }
        [JsonPropertyName("average_duration_of_episode")]
        public int? AverageDurationOfEpisode { get; set; }

        public string Title { get => Name.Main; }
        public string PosterUrl { get => Static.ToFullUrl(!string.IsNullOrEmpty(Poster.Optimized.Src) ? Poster.Optimized.Src : Poster.Src); }
        public string ThumbnailUrl { get => Static.ToFullUrl(!string.IsNullOrEmpty(Poster.Optimized.Thumbnail) ? Poster.Optimized.Thumbnail : Poster.Thumbnail); }
        public string InFavorites { get => AddedInUsersFavorites > 1000 ? $"{AddedInUsersFavorites / 1000.0}K" : $"{AddedInUsersFavorites}"; }
        public string ShortDescription
        {
            get
            {
                if (string.IsNullOrEmpty(Description)) return Description;
                return Description.Length <= 200 ? Description : Description[..200].TrimEnd() + "...";
            }
        }
    }
}
