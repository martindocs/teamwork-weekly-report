# Teamwork Weekly Task Report Automation

> **C# console application** that pulls weekly task data from Teamwork and generates an automated CSV report, updated every 10 minutes via Windows Task Scheduler.

## Overview

This tool connects to the **Teamwork project management API**, retrieves tasks assigned to people for the **current week**, and saves a summary in CSV format. This report can be opened in Excel and provides managers with a quick overview of what team members are working on — without needing to dig into Teamwork manually.

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
5. **Automatically runs every 10 minutes** using Task Scheduler

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

## Schedule It to Run Automatically
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

## Status
✅ In Use at Work
📅 Last Updated: August 2025