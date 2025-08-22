using System.Text.Json;
using System.Threading.Tasks;
using TeamworkWeeklReport.Utils;
using TeamworkWeeklyReport.Models.Teamwork;

namespace TeamworkWeeklyReport.Services
{
    
    public class WorkingOnTasks{
    
        private readonly HttpClient _httpClient;
        private readonly List<Projects> _allProjects;
        private const string WorkingOnTagId = "145044";
        private const string IncludeLastComment = "include=comments";

        public WorkingOnTasks(HttpClient httpClient, List<Projects> allProjects)
        {
            _httpClient = httpClient;
            _allProjects = allProjects;
        }

        public async Task<List<Tasks>> TotalWorkingOnTasks(string baseUrl){

            var allWorkingtasks = new List<Tasks>();

            try
            {
                foreach (var project in _allProjects)
                {
                    string workingOnTaskUrl = $"{baseUrl}/projects/api/v3/projects/{project.Id}/tasks.json?tagIds={WorkingOnTagId}&{IncludeLastComment}";
                    
                    // GET all working tasks per project
                    using var response = await _httpClient.GetAsync(workingOnTaskUrl);

                    response.EnsureSuccessStatusCode();

                    using var stream = await response.Content.ReadAsStreamAsync();

                    // ENSURE ALL PAGES ARE LOADED AS DEFAULT IS TO 50 ON EACH REQUEST
                    var workingOnTaskResponse = await JsonSerializer.DeserializeAsync<WorkingOnTaskResponse>(stream);

                    if (workingOnTaskResponse?.Tasks == null)
                    {
                        Console.WriteLine("No projects found.");
                        continue;
                    }

                    // Create dictionary that have ids of corresponding taskIds
                    var latestComment = workingOnTaskResponse.taskComments.comment.Values.ToDictionary(commentId => commentId.CommentId, commentValue => commentValue.TaskLastComment);
                    
                    
                   
                    foreach (var task in workingOnTaskResponse.Tasks)
                    {
                        int taskId = task.Id;
                        string name = task.Name;
                        int progress = task.Progress;
                        string startDate = task.StartDate;
                        string dueDate = task.DueDate;

                        // Fast lookup, compare taskIds with comments Ids. If match return as text in not return null
                        latestComment.TryGetValue(taskId, out var commentText);
                        
                        allWorkingtasks.Add(new Tasks
                        {
                            Id = taskId,
                            Name = name,
                            Progress = progress,
                            StartDate = startDate,
                            DueDate = dueDate,
                            UserComment = RemoveTags.TagRemover(commentText), // will be null if not found
                        });
                    }
                }

                return allWorkingtasks;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
