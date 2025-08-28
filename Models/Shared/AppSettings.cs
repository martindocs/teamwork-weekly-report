namespace TeamworkWeeklyReport.Models.Shared
{
    public class AppSettings
    {
        public string Version { get; set; }
        public TeamworkConfig Teamwork { get; set; }
        public List<UsersConfig> Users { get; set; }
    }

    public class TeamworkConfig
    {
        public string BaseUrl { get; set; }
        //public List<long> ProjectsIds { get; set; }
    }

    public class UsersConfig
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Sheet { get; set; }
    }
    
}
