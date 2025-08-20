using Microsoft.Extensions.Configuration;
using TeamworkWeeklyReport.Models.Shared;

namespace TeamworkWeeklyReport.Services
{
    // Non static ConfigManager because a static class cannot have any instance members.
    public class ConfigManager
    {
        // Instead I use static Constructor and static members for more future flexibility
        private static readonly IConfigurationRoot _configuration;
        private static readonly AppSettings _appSettings;

        static ConfigManager()
        {
            // Determinate base path
            var devRootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".."));

            var jsonSettingsPath = Path.Combine(devRootPath, "appsettings.Development.json");

            string basePath;

            if (File.Exists(jsonSettingsPath)) { 
                basePath = devRootPath;
            }
            else
            {
                basePath = AppContext.BaseDirectory;
            }

            _configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.Development.json")
                .Build();
            
            _appSettings = _configuration.Get<AppSettings>();           
        }

        public static AppSettings Settings => _appSettings;

        public string GetSettings(string key)
        {
            return _configuration[key];
        }
    }
}
