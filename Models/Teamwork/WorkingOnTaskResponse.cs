using System.Text.Json.Serialization;

namespace TeamworkWeeklyReport.Models.Teamwork
{
    // Working on tasks
    public class WorkingOnTaskResponse // only needed when deserializing the response
    {
        [JsonPropertyName("tasks")]
        public List<Tasks> Tasks { get; set; } = new();
    }

    public class Tasks // can be used to return object
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("startDate")]
        public string StartDate { get; set; }

        [JsonPropertyName("dueDate")]
        public string DueDate { get; set; }

        [JsonPropertyName("progress")]
        public int Progress { get; set; }

        public long ProjectId { get; set; }

        public long ProjectOwnerId { get; set; }
    }
}
