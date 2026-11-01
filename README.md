# StudentManagementSystem

A small ASP.NET Core Web API for managing students, courses, and course enrollments.

## What it does

The API exposes endpoints to:

- Manage students (create, read, update, delete).
- Manage courses (create, read, update, delete).
- Enroll students in courses, list a student's courses, list a course's students, and remove an enrollment.

## Technology stack

- C# / .NET 10
- ASP.NET Core Web API (controller-based)
- Entity Framework Core
- SQLite
- Swagger / OpenAPI (Swashbuckle)
- xUnit (test project)

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Solution structure

```
StudentManagementSystem.sln
src/
  StudentManagementSystem.Api/      # ASP.NET Core Web API
    Controllers/                    # Students, Courses, Enrollments controllers
    Models/                         # Student, Course, Enrollment entities
    Data/                           # AppDbContext and seed data
tests/
  StudentManagementSystem.Tests/    # xUnit test project
```

## Restore dependencies

```bash
dotnet restore
```

## Build the solution

```bash
dotnet build
```

## Run the API

```bash
dotnet run --project src/StudentManagementSystem.Api
```

By default the API listens on the URL(s) printed in the console output (see `src/StudentManagementSystem.Api/Properties/launchSettings.json` for the configured profiles).

## Access Swagger / OpenAPI

Once the API is running, open:

```
http://localhost:<port>/swagger
```

to browse and try out the available endpoints.

## Database initialization

The API uses SQLite. On startup it calls `Database.EnsureCreated()` to create the database file (`studentmanagement.db`, configured via the `DefaultConnection` connection string in `appsettings.json`) if it doesn't already exist, and then seeds it with a deterministic set of sample students, courses, and enrollments if the database is empty. Restarting the application does not duplicate the seed data.

To reset the database, stop the application and delete the `studentmanagement.db` file; it will be recreated and reseeded on the next run.

## Run tests

```bash
dotnet test
```
