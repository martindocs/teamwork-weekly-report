using System.Globalization;

namespace TeamworkWeeklyReport.Utils
{
    public static class WeekOfTheYear
    {
        public static string CurrentWeek()
        {
            DateTime today = DateTime.Today;
            return ISOWeek.GetWeekOfYear(today).ToString();
        }
    }
}
