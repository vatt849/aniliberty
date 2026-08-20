using aniliberty.Api.Data.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using System;
using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases.Episodes
{
    public partial class Episode : ObservableObject
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("ordinal")]
        public decimal Ordinal { get; set; }
        [JsonPropertyName("opening")]
        public Interval Opening { get; set; }
        [JsonPropertyName("ending")]
        public Interval Ending { get; set; }
        [JsonPropertyName("preview")]
        public Poster Preview { get; set; }
        [JsonPropertyName("hls_480")]
        public string HLS480 { get; set; }
        [JsonPropertyName("hls_720")]
        public string HLS720 { get; set; }
        [JsonPropertyName("hls_1080")]
        public string HLS1080 { get; set; }
        [JsonPropertyName("duration")]
        public int Duration { get; set; }
        [JsonPropertyName("rutube_id")]
        public string RutubeID { get; set; }
        [JsonPropertyName("youtube_id")]
        public string YoutubeID { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonPropertyName("sort_order")]
        public int SortOrder { get; set; }
        [JsonPropertyName("name_english")]
        public string NameEnglish { get; set; }

        public string Title { get => $"Серия {Ordinal}{(!string.IsNullOrEmpty(Name) ? $" - {Name}" : "")}"; }
        public string HLSDescr { get => $"{(HLS1080 != "" ? "1080" : "")} {(HLS720 != "" ? "720" : "")} {(HLS480 != "" ? "480" : "")}".Trim(); }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NotViewed))]
        public partial bool Viewed { get; set; }

        public bool NotViewed { get => !Viewed; }
        public Visibility ViewedVisibility() => Viewed ? Visibility.Visible : Visibility.Collapsed;
    }
}
