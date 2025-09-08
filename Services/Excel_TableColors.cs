using TeamworkWeeklyReport.Models.App;
using TeamworkWeeklyReport.Utils;

namespace TeamworkWeeklyReport.Services
{
   public class Excel_TableColors{

        public DueDate DueDate(DateTime date)
        {

            var colors = new List<DueDate>()
            {
                new DueDate{R = 218, G = 242, B = 208}, // green
                new DueDate{R = 252, G = 223, B = 134}, // yellow
                new DueDate{R = 255, G = 209, B = 209}, // red
                new DueDate{R = 255, G = 255, B = 255} // no color
            };

            var today = FormatDate.TodayDate();
            DateTime tomorrow = today.AddDays(1);
            DateTime yesterday = today.AddDays(-1);

            if (date == today)
            {
                return colors[0];
            }
            else if (date == tomorrow)
            {
                return colors[1];
            }
            else if (date <= yesterday)
            {
                return colors[2];
            }
            else
            {
                return colors[3];
            }
        }
    }
}
