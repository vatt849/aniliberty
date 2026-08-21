using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Common
{
    public class Pagination
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("per_page")]
        public int PerPage { get; set; }
        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }
        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
        [JsonPropertyName("links")]
        public PaginationLinks Links { get; set; }

        public bool HasMore { get => Links.Next is not null; }
    }

    public class PaginationLinks
    {
        [JsonPropertyName("previous")]
        public string? Previous { get; set; }
        [JsonPropertyName("next")]
        public string? Next { get; set; }
    }
}
