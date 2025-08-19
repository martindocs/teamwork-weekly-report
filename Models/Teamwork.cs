
using System.Text.Json.Serialization;

namespace TeamworkWeeklyReport.Models
{    
    // Working on tasks
    public class WorkingOnTaskResponse{
        [JsonPropertyName("tasks")]
        public List<Tasks> Tasks { get; set; } = new();
    }
    public class Tasks{
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("startDate")]
        public string StartDate { get; set; }

        [JsonPropertyName("dueDate")]
        public string DueDate { get; set; }

        [JsonPropertyName("priority")]
        public string Priority { get; set; } 
        
        [JsonPropertyName("progress")]
        public int Progress { get; set; }        
    }


    // Active projects
    public class ProjectsStatsResponse
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }
    public class Data{
        [JsonPropertyName("value")]
        public int Value { get; set; }
    }

    // All projects
    public class ProjectsResponse
    {
        [JsonPropertyName("projects")]
        public List<Projects> Projects { get; set; } = new();
    }
    public class Projects
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

}
