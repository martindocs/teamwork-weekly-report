using TeamworkWeeklyReport.Models.Teamwork;
using System.Text.Json;

namespace TeamworkWeeklyReport.Services
{
    public class AllProjects{
        private readonly HttpClient _httpClient;

        public AllProjects(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Projects>> TotalProjects(string baseUrl, int activeProjects){

            string projectsUrl = $"{baseUrl}/projects/api/v3/projects.json?pageSize={activeProjects}";
            // List of Projects Id's
            List<Projects> projectsDetails = new List<Projects>();

            try
            {
                // GET all projects
                using (var response = await _httpClient.GetAsync(projectsUrl))
                {
                    response.EnsureSuccessStatusCode();
               
                    using var stream = await response.Content.ReadAsStreamAsync();
              
                    var projectsResponse = await JsonSerializer.DeserializeAsync<Response_Projects>(stream);

                    if (projectsResponse?.Projects == null)
                    {
                        Console.WriteLine("No projects found.");
                        return null;
                    }

                    // Select Id and Name and create dictionary with user ids and their names
                    var projectOwner = ConfigManager.Settings.Users.Select(user => new { user.Id, user.Name }).ToDictionary(user => user.Id, user => user.Name);

                    foreach (var project in projectsResponse.Projects)
                    {
                        // Fast lookup, compare project owner Ids with users Ids. If match return as text in not return null
                        projectOwner.TryGetValue(project.ProjectOwnerId, out var ProjectOwnerName);

                        projectsDetails.Add(new Projects{
                            Id = project.Id,
                            Name = project.Name,
                            ProjectOwnerId = project.ProjectOwnerId,
                            ProjectOwnerName = ProjectOwnerName
                        });
                    }
                
                    return projectsDetails;
                }
            }
            catch (HttpRequestException ex)
            {
                throw;
            }

        }
    }
}
