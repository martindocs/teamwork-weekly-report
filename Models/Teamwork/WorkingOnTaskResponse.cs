using System.Text.Json.Serialization;

namespace TeamworkWeeklyReport.Models.Teamwork
{
    // Working on tasks
    public class WorkingOnTaskResponse // only needed when deserializing the response
    {
        [JsonPropertyName("tasks")]
        public List<Tasks> Tasks { get; set; } = new();

        [JsonPropertyName("included")]
        public TaskComments taskComments { get; set; }
    }

    public class Tasks // can be used to return object
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("startDate")]
        public string StartDate { get; set; }

        [JsonPropertyName("dueDate")]
        public string DueDate { get; set; }

        [JsonPropertyName("progress")]
        public int Progress { get; set; }

        public string UserComment { get; set; }
    }

    public class TaskComments{
        [JsonPropertyName("comments")]
        public Dictionary<string, Comment> comment { get; set; } = new();
    }

    public class Comment { 
        [JsonPropertyName("objectId")]
        public int CommentId { get; set; }

        [JsonPropertyName("title")]
        public string TaskLastComment { get; set; }
    
    }
}
