using aniliberty.Api.Data.Releases;
using aniliberty.Api.Data.Releases.Episodes;
using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Schedule
{
    public class Schedule
    {
        [JsonPropertyName("release")]
        public Release Release { get; set; }
        [JsonPropertyName("new_release_episode")]
        public Episode NewReleaseEpisode { get; set; }
        [JsonPropertyName("new_release_episode_ordinal")]
        public int NewReleaseEpisodeOrdinal { get; set; }
    }
}
