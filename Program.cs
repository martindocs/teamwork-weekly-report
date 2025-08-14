
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
List<long> projectsIds = ConfigManager.Settings.Teamwork.ProjectsIds;

using var httpClient = new HttpClient();

// Basic Auth
BasicAuth.SetTeamworkBasicAuth(teamworkPassword, apiToken, httpClient);

foreach (var projectId in projectsIds)
{
    


    // Build string 
    string projectUrl = $"{baseUrl}/projects/{projectId}/tasklists.json";

    // Top level task Id's
    List<string> topLevelTasklistId = new List<string>();

    try
    {
        // GET request - Top level task Id's
        using (var response = await httpClient.GetAsync(projectUrl))
        { 
            response.EnsureSuccessStatusCode();

            // Read JSON file
            using var stream = await response.Content.ReadAsStreamAsync();
       
            var tasklistsResponse = await JsonSerializer.DeserializeAsync<TaskListsResponse>(stream);

            if(tasklistsResponse?.TasksLists == null){

                Console.WriteLine($"No task lists found.");
                continue;
            }

            foreach (var task in tasklistsResponse.TasksLists)
            {
                string? id = task.Id;
                topLevelTasklistId.Add(id);
                Console.WriteLine($"Top level Task found {id}");
            }
        }

        // Loop through each task list Id's and fetch data
        foreach (var tasklistId in topLevelTasklistId)
        {
            // Build string 
            string tasksUrl = $"{baseUrl}" + $"/projects/api/v3/tasklists/{tasklistId}/tasks.json";

            using var response = await httpClient.GetAsync(tasksUrl);
            response.EnsureSuccessStatusCode();

            // Read JSON file
            using var stream = await response.Content.ReadAsStreamAsync();

            var TasksResponse = await JsonSerializer.DeserializeAsync<TasksResponse>(stream);

            if (TasksResponse?.Tasks == null)
            {
                Console.WriteLine($"No tasks found.");
                continue;
            }

            foreach (var task in TasksResponse.Tasks)
            {
                string taskName = task.Name;
                string taskPriority = task.Priority;
                Console.WriteLine($"Task: {taskName}, priority: {taskPriority}");

                //    // Optional: subtasks
                //    //if (task.TryGetProperty("subtasks", out JsonElement subtasks))
                //    //{
                //    //    foreach (var subtask in subtasks.EnumerateArray())
                //    //    {
                //    //        string subtaskName = subtask.GetProperty("content").GetString();
                //    //        Console.WriteLine($"  Subtask: {subtaskName}");
                //    //    }
                //    //}
                //}
            }

        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
        throw;
    }
}
Console.ReadLine();