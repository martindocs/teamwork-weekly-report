using System.Text.Json;
using System.Threading.Tasks;
//using TeamworkWeeklyReport.Utils;
using TeamworkWeeklyReport.Models.Teamwork;

namespace TeamworkWeeklyReport.Services
{
    
    public class WorkingOnTasks{
    
        private readonly HttpClient _httpClient;
        private readonly List<Projects> _allProjects;
        private const string WorkingOnTagId = "145044";

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
                    string workingOnTaskUrl = $"{baseUrl}/projects/api/v3/projects/{project.Id}/tasks.json?tagIds={WorkingOnTagId}";
                    
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
                        long taskId = task.Id;
                        string name = task.Name;
                        int progress = task.Progress;
                        string startDate = task.StartDate;
                        string dueDate = task.DueDate;
                                                
                        allWorkingtasks.Add(new Tasks
                        {
                            Id = taskId,
                            Name = name,
                            Progress = progress,
                            StartDate = startDate,
                            DueDate = dueDate,
                            ProjectId = project.Id,
                            ProjectOwnerId = project.ProjectOwnerId
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
