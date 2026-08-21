using aniliberty.Api.Data.Common;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace aniliberty.Api.Responses;

public class PaginatedResponse<T>
{
    [JsonPropertyName("data")]
    public List<T> List { get; set; }
    [JsonPropertyName("meta")]
    public PaginatedResponseMeta Meta { get; set; }
}

public class PaginatedResponseMeta
{
    [JsonPropertyName("pagination")]
    public Pagination Pagination { get; set; }
}
