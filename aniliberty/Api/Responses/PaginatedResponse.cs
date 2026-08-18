using aniliberty.Api.Data.Common;
using aniliberty.Api.Data.Releases;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace aniliberty.Api.Responses;

public class PaginatedResponse
{
    [JsonPropertyName("data")]
    public List<Release> List { get; set; }
    [JsonPropertyName("meta")]
    public PaginatedResponseMeta Meta { get; set; }
}

public class PaginatedResponseMeta
{
    [JsonPropertyName("pagination")]
    public Pagination Pagination { get; set; }
}
