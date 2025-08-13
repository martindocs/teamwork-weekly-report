
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using TeamworkWeeklyReport.Models;
using TeamworkWeeklyReport.Services;
using static System.Net.WebRequestMethods;

// Load baseUrl and Api key/ Teamwork password
string baseUrl = ConfigManager.Settings.Teamwork.BaseUrl;
string? apiToken = Environment.GetEnvironmentVariable("apiKey");
string? teamworkPassword = Environment.GetEnvironmentVariable("teamworkPassword");

if (string.IsNullOrEmpty(apiToken))
{
    Console.WriteLine("Api Key is null");
    return;
}

// Project ID
long projectID = ConfigManager.Settings.Teamwork.ProjectsIds[0];

// HTTP Client
using var httpClient = new HttpClient();

var tokenBytes = System.Text.Encoding.UTF8.GetBytes($"{apiToken}:{teamworkPassword}");
var tokenBase64 = Convert.ToBase64String(tokenBytes);
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Basic", tokenBase64);

// Build string 
string projectUrl = $"{baseUrl}" + $"/projects/{projectID}/tasklists.json";

// Top level task Id's
List<string> topLevelTasklistId = new List<string>();

try
{
    // GET request
    using (var response = await httpClient.GetAsync(projectUrl))
    { // using bracket to dispose once data fetched. Wrapping all means that even if parsing fails, or an exception is thrown, the response will be disposed immediately.

        response.EnsureSuccessStatusCode();

        // Read JSON file
        using var stream = await response.Content.ReadAsStreamAsync();
        var jsonDoc = await JsonDocument.ParseAsync(stream);
    
        var root = jsonDoc.RootElement;

        //string formattedJson = JsonSerializer.Serialize(json.RootElement, new JsonSerializerOptions
        //{
        //    WriteIndented = true
        //});

        //Console.WriteLine(formattedJson);

        
        if (root.TryGetProperty("tasklists", out JsonElement tasklists))
        {
            foreach (var tasklist in tasklists.EnumerateArray())
            {
                string? id = tasklist.GetProperty("id").GetString();
                topLevelTasklistId.Add(id);
                Console.WriteLine($"Top level Task found {id}");
            }
        }
    }

    // Loop through each tasklist Id'sand fetch data
    foreach (var tasklistId in topLevelTasklistId)
    {
        // Build string 
        string tasksUrl = $"{baseUrl}" + $"/tasklists/{tasklistId}/tasks.json";

        using var response = await httpClient.GetAsync(projectUrl);
        response.EnsureSuccessStatusCode();

        // Read JSON file
        using var stream = await response.Content.ReadAsStreamAsync();
        var jsonDoc = await JsonDocument.ParseAsync(stream);

        //foreach (var task in jsonDoc.RootElement.GetProperty("tasks").EnumerateArray())
        //{
        //}

        //foreach(var task in jsonDoc.RootElement.GetProperty("tasks").EnumerateArray())
        //{
        //        string taskName = task.GetProperty("content").GetString();
        //        Console.WriteLine($"Task: {taskName}");

        //        // Optional: subtasks
        //        if (task.TryGetProperty("subtasks", out JsonElement subtasks))
        //        {
        //            foreach (var subtask in subtasks.EnumerateArray())
        //            {
        //                string subtaskName = subtask.GetProperty("content").GetString();
        //                Console.WriteLine($"  Subtask: {subtaskName}");
        //            }
        //        }
        //    }
    }

}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
    throw;
}
Console.ReadLine();