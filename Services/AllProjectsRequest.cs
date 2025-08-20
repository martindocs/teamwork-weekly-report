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

        public async Task<List<long>> TotalProjects(string baseUrl, int activeProjects){

            string projectsUrl = $"{baseUrl}/projects/api/v3/projects.json?pageSize={activeProjects}";
            // List of Projects Id's
            List<long> projectsIds = new List<long>();

            try
            {
                // GET all projects
                using (var response = await _httpClient.GetAsync(projectsUrl))
                {
                    response.EnsureSuccessStatusCode();
               
                    using var stream = await response.Content.ReadAsStreamAsync();
              
                    var projectsResponse = await JsonSerializer.DeserializeAsync<ProjectsResponse>(stream);

                    if (projectsResponse?.Projects == null)
                    {
                        Console.WriteLine("No projects found.");
                        return null;
                    }

                    foreach (var project in projectsResponse.Projects)
                    {                    
                        projectsIds.Add(project.Id);
                    }
                
                    return projectsIds;
                }
            }
            catch (HttpRequestException ex)
            {
                throw;
            }

        }
    }
}
