# Teamwork Weekly Task Report Automation

> C# console application that interacts with the Teamwork API to generate automated CSV reports summarizing tasks for the current week.

## Overview

This project is a personal tool built as a coding exercise to automate weekly task reporting using the Teamwork.com project management API.

It connects to Teamwork, retrieves task data, and outputs a structured .csv file summarizing current week's tasks — ready to open in Excel. The tool can be automated to run periodically using Windows Task Scheduler.

## Features

- Pulls tasks using Teamwork REST API
- Filters tasks by due date (this week's tasks)
- Extracts task title, assignee, due date, and status
- Saves to a readable `.csv` file
- Automatically runs every 10 minutes (via Windows Task Scheduler)

## Project Structure

```cbash
teamwork-weekly-report/
├── Interfaces/
├── Models/             // Pure data (properties only)
│   ├── TaskItem.cs
│   ├── TeamworkUser.cs
├── Services/           // Logic like API calls, file writing, etc.
│   ├── TeamworkService.cs
│   ├── CsvExporter.cs
├── Output/             // Generated CSV files
│ └── weekly_report.csv
├── App.config          // Configuration: API key and Teamwork URL
├── Program.cs          // Main logic
└── README.md
```

## How It Works

1. **Reads configuration** (API key, Teamwork base URL)
2. **Calls the Teamwork API** to fetch tasks
3. **Filters tasks** to include only those due this week
4. **Writes a CSV file** with:
   - Person name
   - Task title
   - Due date
   - Status
5. **Can be scheduled to run automatically every 10 minutes** using Windows Task Scheduler.

## Setup Instructions
### 1. Clone the Project

```bash
git clone https://github.com/martindocs/teamwork-weekly-report.git
cd teamwork-weekly-report
```

### 2. Add Your Teamwork Credentials
```json
{
  "Teamwork": {
    "ApiKey": "your_teamwork_api_key",
    "BaseUrl": "https://yourcompany.teamwork.com"
  }
}
```

### 3. Build and Run the App
```bash
dotnet run
```

## (Optional) Schedule It to Run Automatically
### 1. Build the app
```bash
dotnet publish -c Release
```

### 2. Open Windows Task Scheduler
- Create a new task

- Set trigger: Every 10 minutes

- Set action: Run the .exe file from the publish folder

## What I Learned
- Using REST APIs in C# with HttpClient

- Parsing JSON responses

- Working with dates and time filtering

- Generating CSV files in C#

- Setting up Task Scheduler to automate a console app

- Structuring and documenting a real-world project

## Future Improvements
- Write to .xlsx with formatting

- Push reports to SharePoint or OneDrive

- Send summary via email to stakeholders

- Add logging and error handling

- Add per-user summaries or charts

## Security & Privacy

* No sensitive credentials are included in this repo.

* API keys and secrets are handled via environment variables or local config files and never committed.

* Sample data and task identifiers are sanitized and generic. No private project data is stored or exposed.

## 📄 License

This project is licensed under the MIT License.

Disclaimer: This software is provided "as is", without warranty of any kind. The author is not liable for any damages or issues caused by the use of this tool. Use at your own risk.

See [LICENSE](LICENSE)
 for details.

## Status
🟢 Maintained as a personal portfolio project

📅 Last updated: August 2025