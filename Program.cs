
using TeamworkWeeklyReport.Services;
using TeamworkWeeklyReport.Utils;

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

using var httpClient = new HttpClient();

// Basic Auth
BasicAuth.SetTeamworkBasicAuth(teamworkPassword, apiToken, httpClient);

try
{
    var activeProjectsCountRequest = new ActiveProjects(httpClient);
    int activeProjectsCount = await activeProjectsCountRequest.TotalActiveProjects(baseUrl);

    var totalProjectsRequest = new AllProjects(httpClient);
    var totalProjects = await totalProjectsRequest.TotalProjects(baseUrl, activeProjectsCount);

    var workingOnTasksRequest = new WorkingOnTasks(httpClient, totalProjects);
    var workingOnTasks = await workingOnTasksRequest.TotalWorkingOnTasks(baseUrl);
    
    // TEST
    foreach (var item in workingOnTasks)
    {
        Console.WriteLine(item.Name);
    }    

}
catch(HttpRequestException ex){
    Console.WriteLine("Api Error: " + ex.Message);
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}

Console.ReadLine();