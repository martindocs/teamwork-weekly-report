
using System.Text.Json.Serialization;

namespace TeamworkWeeklyReport.Models
{
    public class TaskLists
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class TaskListsResponse
    {
        [JsonPropertyName("tasklists")]
        public List<TaskLists> TasksLists { get; set;} = new List<TaskLists>();
    }


    public class Tasks
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("priority")]
        public string Priority { get; set; }
    }

    public class TasksResponse{
        [JsonPropertyName("tasks")]
        public List<Tasks> Tasks { get; set; } = new List<Tasks>();
    }

}
