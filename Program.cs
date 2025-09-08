
using TeamworkWeeklyReport.Services;
using TeamworkWeeklyReport.Utils;
using TeamworkWeeklyReport.Tests.TeamworkAPI;

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

    var projectsIds = totalProjects.Select(project => project.Id).ToList();

    var groupTasks = new GroupTasks(workingOnTasks);
    var groupUserTasks = groupTasks.GroupUserTasks(projectsIds);


    // TODO: DUMMY TEAMWORK API
    //var dummyAPIRequest = new Requests_DummyAPI();
    //var workingOnTasks = dummyAPIRequest.GetProjectsTasks();
    //var projectsIds = dummyAPIRequest.GetActiveProjectsIds(); ;

    //var groupTasks = new GroupTasks(workingOnTasks);
    //var groupUserTasks = groupTasks.GroupUserTasks(projectsIds);

    // SAVE TO EXCEL
    var excel = new Excel_Table(groupUserTasks, new Excel_TableColors());
    excel.CreateTable();
    

}
catch (HttpRequestException ex){
    Console.WriteLine("Api Error: " + ex.Message);
}
catch (Exception ex)
{
    Console.WriteLine("General Error: " + ex.Message);
}

Console.ReadLine();