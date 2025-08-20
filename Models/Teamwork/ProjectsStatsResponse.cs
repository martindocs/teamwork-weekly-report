using System.Text.Json.Serialization;

namespace TeamworkWeeklyReport.Models.Teamwork
{
    // Active projects
    public class ProjectsStatsResponse
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }
    public class Data
    {
        [JsonPropertyName("value")]
        public int Value { get; set; }
    }
    
}
