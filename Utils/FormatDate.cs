using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamworkWeeklyReport.Utils
{
    public static class FormatDate
    {
        public static DateTime DateToUTC (string currentDate)
        {
            // currentDate example:"2025-09-08T14:00:00-04:00"
            DateTime date = DateTime.Parse(currentDate, null, System.Globalization.DateTimeStyles.AdjustToUniversal); // converts the time to UTC. Typically use this when working with APIs, logs, or data that include time zone info, and you want to normalize all times to UTC.

            return date;
        }

        public static DateTime TodayDate() => DateTime.Today;
    }
}
