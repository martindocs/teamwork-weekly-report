using TeamworkWeeklyReport.Models.Teamwork;
using System.Text.Json;

namespace TeamworkWeeklyReport.Services
{
    public class ActiveProjects{
        private readonly HttpClient _httpClient;

        public ActiveProjects(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> TotalActiveProjects(string baseUrl){            
            string projectsStatsUrl = $"{baseUrl}/projects/api/v3/projects/metrics/active.json";

            try
            {
                // GET all projects stats
                using (var response = await _httpClient.GetAsync(projectsStatsUrl))
                {
                    response.EnsureSuccessStatusCode();

                    // Read the content from the server and returned as a string
                    // "using" avoids loading the entire response into memory as a string first.
                    using var stream = await response.Content.ReadAsStreamAsync();

                    var projectsStatsResponse = await JsonSerializer.DeserializeAsync<Response_ActiveProjects>(stream);

                    if (projectsStatsResponse?.Data == null)
                    {
                        Console.WriteLine("No information about projects found.");
                        return 0;
                    }

                    return projectsStatsResponse.Data.Value;
                }
            }
            catch (HttpRequestException ex)
            {
                throw;
            }
        }
    }
}
