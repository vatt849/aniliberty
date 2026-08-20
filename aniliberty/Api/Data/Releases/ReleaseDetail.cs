using aniliberty.Api.Data.Genres;
using aniliberty.Api.Data.Releases.Episodes;
using aniliberty.Api.Data.Releases.Members;
using aniliberty.Api.Data.Releases.Torrents;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases
{
    public class ReleaseDetail : Release
    {
        [JsonPropertyName("genres")]
        public List<Genre> Genres { get; set; }
        [JsonPropertyName("members")]
        public List<Member> Members { get; set; }
        [JsonPropertyName("episodes")]
        public List<Episode> Episodes { get; set; }
        [JsonPropertyName("torrents")]
        public List<Torrent> Torrents { get; set; }
        [JsonPropertyName("sponsor")]
        public Sponsor Sponsor { get; set; }
    }
}
