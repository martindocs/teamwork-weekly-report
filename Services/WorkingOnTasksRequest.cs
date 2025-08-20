using System.Text.Json;
using TeamworkWeeklyReport.Models.Teamwork;

namespace TeamworkWeeklyReport.Services
{
    
    public class WorkingOnTasks{
    
        private readonly HttpClient _httpClient;
        private readonly List<long> _allTasks;
        private const string WorkingOnTag = "?tagIds=145044";

        public WorkingOnTasks(HttpClient httpClient, List<long> allTasks)
        {
            _httpClient = httpClient;
            _allTasks = allTasks;
        }

        public async Task<List<Tasks>> TotalWorkingOnTasks(string baseUrl){

            var allWorkingtasks = new List<Tasks>();

            try
            {
                foreach (var project in _allTasks)
                {
                    string workingOnTaskUrl = $"{baseUrl}/projects/api/v3/projects/{project}/tasks.json{WorkingOnTag}";

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
                    foreach (var task in workingOnTaskResponse.Tasks)
                    {
                        int id = task.Id;
                        string name = task.Name;
                        string priority = task.Priority;
                        int progress = task.Progress;
                        string startDate = task.StartDate;
                        string dueDate = task.DueDate;

                        allWorkingtasks.Add(new Tasks
                        {
                            Id = id,
                            Name = name,
                            Priority = priority,
                            Progress = progress,
                            StartDate = startDate,
                            DueDate = dueDate
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
