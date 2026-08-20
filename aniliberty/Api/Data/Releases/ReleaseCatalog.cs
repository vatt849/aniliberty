using aniliberty.Api.Data.Genres;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Releases
{
    public class ReleaseCatalog : Release
    {
        [JsonPropertyName("genres")]
        public List<Genre> Genres { get; set; }
    }
}
