namespace TeamworkWeeklyReport.Utils
{
    public static class FilePath
    {
        public static string GetPath(string path)
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, string.Empty);
            }

            return path;
        }
    }
}