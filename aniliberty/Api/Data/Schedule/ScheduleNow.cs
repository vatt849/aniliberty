using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace aniliberty.Api.Data.Schedule
{
    public class ScheduleNow
    {
        [JsonPropertyName("today")]
        public List<Schedule> Today { get; set; }
        [JsonPropertyName("tomorrow")]
        public List<Schedule> Tomorrow { get; set; }
        [JsonPropertyName("yesterday")]
        public List<Schedule> Yesterday { get; set; }
    }
}
