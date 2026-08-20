using aniliberty.Api.Data.Genres;
using aniliberty.Api.Data.Releases.Episodes;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases
{
    public class ReleaseLatest : Release
    {
        [JsonPropertyName("genres")]
        public List<Genre> Genres { get; set; }
        [JsonPropertyName("latest_episode")]
        public Episode LatestEpisode { get; set; }
    }
}
