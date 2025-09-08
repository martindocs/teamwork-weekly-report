# Teamwork Weekly Task Report Automation

> C# console app built to help my team get clearer weekly updates from Teamwork.

## Overview

This project started with a simple idea: make it easier for my engineering team to see what everyone's working on. We were using [Teamwork.com](https://www.teamwork.com)
, but with so many tasks flying around, it was hard for managers to get a clear weekly summary.

I thought — why not build a little tool that automates all of this?

I started by building a C# console app that:

* Connects to the Teamwork API
* Pulls task data
* Filters out only the useful stuff (like tasks tagged working-on)
* Saves it to a Excel `.xlsx` report

The app worked great. It pulled the data, formatted it nicely, and could even run on a schedule using Windows Task Scheduler.

But then I hit a wall.
Our IT department flagged it as a security risk (no .exe files allowed on servers), so the project couldn’t go live internally.

So I pivoted.

Instead of dropping the idea, I rebuilt the report using Excel, with:

* Power Query to pull and transform data
* A clean and simple user interface
* A few VBA macros for added functionality

And it worked!
The Excel version ended up being even more feature-rich, and I delivered it to the engineering team.

So while the C# version became more of a portfolio project, the overall goal, making our task reporting clearer and easier was still achieved 💪

## Features

- Pulls tasks using Teamwork REST API
- Filters tasks by due date 
- Extracts project name, task title, assignee, due date, and task progress
- Saves to a readable `.xlsx` file

## Project Structure

```cbash
teamwork-weekly-report/
├── Properties/                 // Local settings
├── Models/                     // Pure data (properties only)
│   ├── App/
│   ├── Shared/
│   ├── Teamwork/
├── Output/                     // Generated Excel files 
├── Services/                   // Logic like API calls, file writing, etc.
├── Test/                       // Dummy test files
├── Utils/                      // Resuable logic used across app
├── appsettings.Template.json   // Configuration: API key and Teamwork URL
├── LICENSE
├── Program.cs                  // Main logic
└── README.md

```

## How It Works

1. **Reads configuration** (API key, Teamwork base URL)
2. **Calls the Teamwork API** to fetch tasks
3. **Filters tasks** to include only those due this week
4. **Writes a Excel file** with:
   - Project name
   - Person name
   - Task title
   - Due date
   - Status

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

- Generating Excel files in C#

- Structuring and documenting a real-world project

## Future Improvements
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

📅 Last updated: September 2025