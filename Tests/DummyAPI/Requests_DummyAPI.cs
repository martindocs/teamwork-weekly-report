using System.Text.Json;
using TeamworkWeeklyReport.Utils;
using TeamworkWeeklyReport.Models.Teamwork;

namespace TeamworkWeeklyReport.Tests.TeamworkAPI
{
    public class Requests_DummyAPI        
    {
        public List<long> GetActiveProjectsIds()
        {
            string path = FilePath.GetPath(Constants.PROJECTS_FILE_PATH);

            try
            {
                string json = File.ReadAllText(path);
                var getJSONfile = JsonSerializer.Deserialize<List<long>>(json); 

                return getJSONfile;
            }
            catch (JsonException ex)
            {
                throw;
            }
        }

        public List<Tasks> GetProjectsTasks()
        {
            string path = FilePath.GetPath(Constants.TASKS_FILE_PATH);

            try
            {
                string json = File.ReadAllText(path);
                var getJSONfile = JsonSerializer.Deserialize<List<Tasks>>(json);

                return getJSONfile;
            }
            catch (JsonException ex)
            {
                throw;
            }
        }
    }
}
