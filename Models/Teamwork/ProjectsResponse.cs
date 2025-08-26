using System.Text.Json.Serialization;

namespace TeamworkWeeklyReport.Models.Teamwork
{
    // All projects
    public class ProjectsResponse
    {
        [JsonPropertyName("projects")]
        public List<Projects> Projects { get; set; } = new();
    }
    public class Projects
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("ownedBy")]
        public int ProjectOwnerId { get; set; }

        public string ProjectOwnerName { get; set; }
    }
}
