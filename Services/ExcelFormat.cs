using ClosedXML.Excel;
using TeamworkWeeklyReport.Models.Teamwork;
using TeamworkWeeklyReport.Utils;
using TeamworkWeeklyReport.Models.App;
using Color = System.Drawing.Color;
using System.Globalization;

namespace TeamworkWeeklyReport.Services
{
    public class ExcelFormat
    {
        private readonly Dictionary<long, List<Tasks>> _projectOwnerIds;

        public ExcelFormat(Dictionary<long, List<Tasks>> projectOwnerIds)
        {
            _projectOwnerIds = projectOwnerIds;
        }

        public void CreateExcelTable()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Week 36");

                worksheet.Columns().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Columns().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // Header
                worksheet.Cell("B1").Value = $"18-22 Aug / Week {WeekOfYear()}";
                worksheet.Row(1).Height = 30;
                worksheet.Cell("B1").Style.Fill.BackgroundColor = XLColor.FromColor(Color.FromArgb(218, 233, 248));
                worksheet.Cell("B1").Style.Font.FontSize = 14;
                worksheet.Row(1).Style.Font.Bold = true;
                worksheet.Range("B1:F1").Merge();

                // sub-Header
                worksheet.Cell("B2").Value = "Project";
                worksheet.Cell("C2").Value = "Task";
                worksheet.Cell("D2").Value = "Owner";
                worksheet.Cell("E2").Value = "Due Date";
                worksheet.Cell("F2").Value = "Progress [%]";

                // sub-Headers colors
                worksheet.Cell("B2").Style.Fill.BackgroundColor = XLColor.FromColor(Color.FromArgb(232, 232, 232));
                worksheet.Cell("C2").Style.Fill.BackgroundColor = XLColor.FromColor(Color.FromArgb(232, 232, 232));
                worksheet.Cell("D2").Style.Fill.BackgroundColor = XLColor.FromColor(Color.FromArgb(232, 232, 232));
                worksheet.Cell("E2").Style.Fill.BackgroundColor = XLColor.FromColor(Color.FromArgb(232, 232, 232));
                worksheet.Cell("F2").Style.Fill.BackgroundColor = XLColor.FromColor(Color.FromArgb(232, 232, 232));

                worksheet.Row(2).Style.Font.Bold = true;

                int row = 3;
                int tempRow = 3;
                string? tempProjectName = ""; 
                foreach (var projectOwnerId in _projectOwnerIds)
                {
                    string? projectName = "";
                    string? projectOwnerName = "";
                    foreach (var cell in projectOwnerId.Value)
                    {
                        // Project Name
                        if (projectName == "")
                        {
                            projectName = $"{cell.ProjectName}";
                            worksheet.Cell(row, 2).Value = projectName;
                            tempProjectName = projectName;
                        }

                        // Tasks List
                        worksheet.Cell(row, 3).Value = $"{cell.Name}";
                        worksheet.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                       
                        // Owner Name
                        if (projectOwnerName == ""){
                            projectOwnerName = ConfigManager.Settings.Users.FirstOrDefault(user => user.Id == projectOwnerId.Key)?.Name;
                            worksheet.Cell(row, 4).Value = projectOwnerName;                        
                        }

                        // Due Date
                        if(cell.DueDate != null){
                            var date = FormatDate(cell.DueDate);
                            var dateColor = DueDate(date);
                            worksheet.Cell(row, 5).Value = date;
                            worksheet.Cell(row, 5).Style.Fill.BackgroundColor = XLColor.FromColor(Color.FromArgb(
                                dateColor.R, 
                                dateColor.G, 
                                dateColor.B
                              ));
                        }

                        // Progress
                        worksheet.Cell(row, 6).Value = cell.Progress;

                        worksheet.Row(row).Style.Font.Bold = true;

                        row++;
                    }
                   
                    worksheet.Range($"B{tempRow}:B{row - 1}").Merge();
                    worksheet.Range($"D{tempRow}:D{row - 1}").Merge();
                    tempRow = row;                    
                     
                }

                var usedRange = worksheet.RangeUsed();

                if (usedRange != null)
                {
                    usedRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    usedRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    usedRange.Cells().Style.Font.FontName = "Aptos Narrow Bold";
                }

                //worksheet.Rows().AdjustToContents();

                worksheet.Column(2).Width = 25;
                worksheet.Column(2).Style.Alignment.WrapText = true;
                worksheet.Column(4).Width = 25;
                worksheet.Column(4).Style.Alignment.WrapText = true;

                worksheet.SheetView.FreezeRows(1);
                worksheet.SheetView.FreezeRows(2);

                worksheet.Column(3).AdjustToContents();
                //worksheet.Column(5).AdjustToContents();
                worksheet.Column(5).Width = 12;
                worksheet.Column(6).AdjustToContents();  
                
                workbook.SaveAs(FilePath.GetPath(FilePaths.EXCEL_FILE_PATH));

                Console.WriteLine("File Saved!");
            }

        }

        public int WeekOfYear(){
            DateTime today = DateTime.Today;
            return ISOWeek.GetWeekOfYear(today);
        }

        public DateTime FormatDate(string dueDate){
            DateTime date = DateTime.Parse(dueDate, null, System.Globalization.DateTimeStyles.AdjustToUniversal);

            return date;
        }

        public DueDateColors DueDate(DateTime date) {
            
            var colors = new List<DueDateColors>()
            {
                new DueDateColors{R = 218, G = 242, B = 208}, // green
                new DueDateColors{R = 252, G = 223, B = 134}, // yellow
                new DueDateColors{R = 255, G = 209, B = 209}, // red
                new DueDateColors{R = 255, G = 255, B = 255} // no color
            };
            DateTime today = DateTime.Today;
            DateTime tomorrow = DateTime.Today.AddDays(1);
            DateTime yesterday = DateTime.Today.AddDays(-1);
           
            if(date == today){
                return colors[0];
            }else if (date == tomorrow){  
                return colors[1];
            } else if(date <= yesterday){
                return colors[2];            
            }else{
                return colors[3];            
            
            }
        }
    }
}

