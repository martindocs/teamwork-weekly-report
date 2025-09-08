using ClosedXML.Excel;
using TeamworkWeeklyReport.Models.Teamwork;
using TeamworkWeeklyReport.Utils;
using TeamworkWeeklyReport.Models.App;
using Color = System.Drawing.Color;
using System.Globalization;

namespace TeamworkWeeklyReport.Services
{
    public class Excel_Table
    {
        private readonly Dictionary<long, List<Tasks>> _projectOwnerIds;     
        private readonly Excel_TableColors _tableDueDateColors;

        public Excel_Table(Dictionary<long, List<Tasks>> projectOwnerIds, Excel_TableColors tableDueDateColors)
        {
            _projectOwnerIds = projectOwnerIds;
            _tableDueDateColors = tableDueDateColors;
        }

        public void CreateTable()
        {
            using (var workbook = new XLWorkbook())
            {
                string currentWeek = WeekOfTheYear.CurrentWeek();

                var worksheet = workbook.Worksheets.Add(currentWeek);

                worksheet.Columns().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Columns().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // Header
                worksheet.Cell("B1").Value = $"18-22 Aug / Week {currentWeek}";
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
                            var date = FormatDate.DateToUTC(cell.DueDate);
                            var dateColor = _tableDueDateColors.DueDate(date);
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

                worksheet.Column(2).Width = 25;
                worksheet.Column(2).Style.Alignment.WrapText = true;
                worksheet.Column(4).Width = 25;
                worksheet.Column(4).Style.Alignment.WrapText = true;

                worksheet.SheetView.FreezeRows(1);
                worksheet.SheetView.FreezeRows(2);

                worksheet.Column(3).AdjustToContents();
                worksheet.Column(5).Width = 12;
                worksheet.Column(6).AdjustToContents();  
                
                workbook.SaveAs(FilePath.GetPath(FilePaths.EXCEL_FILE_PATH));

                Console.WriteLine("File Saved!");
            }

        }
        
    }
}

