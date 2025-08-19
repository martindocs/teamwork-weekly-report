
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using TeamworkWeeklyReport.Models;
using TeamworkWeeklyReport.Services;
using TeamworkWeeklyReport.Utils;
using static System.Net.WebRequestMethods;

// Load baseUrl and Api key/ Teamwork password
string baseUrl = ConfigManager.Settings.Teamwork.BaseUrl;
string? apiToken = Environment.GetEnvironmentVariable("apiKey");
string? teamworkPassword = Environment.GetEnvironmentVariable("teamworkPass");

if (string.IsNullOrEmpty(apiToken))
{
    Console.WriteLine("Api Key is null");
    return;
}

if (string.IsNullOrEmpty(teamworkPassword))
{
    Console.WriteLine("Password is null");
    return;
}

// Project ID
//List<long> projectsIds = ConfigManager.Settings.Teamwork.ProjectsIds;

using var httpClient = new HttpClient();

// Basic Auth
BasicAuth.SetTeamworkBasicAuth(teamworkPassword, apiToken, httpClient);

try
{
    // Total active projects
    int activeProjects = 0;
     
    string projectsStatsUrl = $"{baseUrl}/projects/api/v3/projects/metrics/active.json";

    // GET all projects stats
    using (var response = await httpClient.GetAsync(projectsStatsUrl)){
        response.EnsureSuccessStatusCode();

        // Read the content from the server and returned as a string
        // "using" avoids loading the entire response into memory as a string first.
        using var stream = await response.Content.ReadAsStreamAsync();
        
        var projectsStatsResponse = await JsonSerializer.DeserializeAsync<ProjectsStatsResponse>(stream);

        if(projectsStatsResponse?.Data == null){
            Console.WriteLine("No information about projects found.");
            return;
        }

        activeProjects = projectsStatsResponse.Data.Value;
               
    }
    
    
    string projectsUrl = $"{baseUrl}/projects/api/v3/projects.json?pageSize={activeProjects}";
    // List of Projects Id's
    List<long> projectsIds = new List<long>();

    // GET all projects
    using (var response = await httpClient.GetAsync(projectsUrl)){
    
        response.EnsureSuccessStatusCode();
       
        using var stream = await response.Content.ReadAsStreamAsync();

        var projectsResponse = await JsonSerializer.DeserializeAsync<ProjectsResponse>(stream);

        if (projectsResponse?.Projects == null)
        {
            Console.WriteLine("No projects found.");
            return;
        }

        foreach (var project in projectsResponse.Projects)
        {
            int id = project.Id;
            //string? name = project.Name;
            projectsIds.Add(id);
        }
        
    }

    foreach (var project in projectsIds)
    {    
        string workingOnTaskUrl = $"{baseUrl}/projects/api/v3/projects/{project}/tasks.json?tagIds=145044";

        // GET all working tasks per project
        using var response = await httpClient.GetAsync(workingOnTaskUrl);

        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();

        // ENSURE ALL PAGES ARE LOADED AS DEFAULT IS TO 50 ON EACH REQUEST
        var workingOnTasksResponse = await JsonSerializer.DeserializeAsync<WorkingOnTaskResponse>(stream);

        if (workingOnTasksResponse?.Tasks == null)
        {
            Console.WriteLine("No projects found.");
            continue;
        }

        foreach (var task in workingOnTasksResponse.Tasks)
        {
            int? id = task.Id;
            string? name = task.Name;
            string? priority = task.Priority;
            int? progress = task.Progress;
            string startDate = task.StartDate;
            string dueDate = task.DueDate;

            Console.WriteLine($"Task id: {id}, name: {name}, priority: {priority}, progress: {progress}, start date: {startDate}, due date: {dueDate}");
        }
    }

}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}

Console.ReadLine();