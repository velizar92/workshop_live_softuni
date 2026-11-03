# CLAUDE.md

## 1. Project Overview

`StudentManagementSystem` is an ASP.NET Core Web API for managing students, courses, and student enrollments.

### Technology Stack
- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- xUnit for unit testing

Before making changes, inspect the existing solution and follow its established structure unless the task explicitly requires a change.

## 2. Development Workflow

### Understand Before Changing
- Read the relevant controllers, models, database context, and tests before modifying code.
- Trace the existing request flow and understand the current behavior.
- Check related code and existing conventions before introducing new patterns.
- Do not assume that existing behavior is incorrect without investigating it.
- Keep changes focused on the requested GitHub Issue.

### Work Through GitHub Issues
- Every development task must be associated with a GitHub Issue whenever applicable.
- Read the Issue description, acceptance criteria, labels, and dependencies before starting.
- If requirements are ambiguous, investigate the code and ask for clarification when necessary.
- Do not silently expand the scope of an Issue.
- In some case is also possible to have pull request without working on particular issue

### Branch and Pull Request Policy — Mandatory

**Never make project changes directly on `main`. Every change must be developed on a separate branch and merged into `main` only through a Pull Request.**

1. Ensure the working tree is clean before starting a new task. Do not overwrite, discard, or commit unrelated user changes.
2. Start from an up-to-date `main` branch and create a dedicated branch for the Issue.
3. Use descriptive branch names, for example:
   - `fix/prevent-duplicate-enrollments`
   - `fix/course-capacity`
   - `refactor/enrollment-service`
   - `test/enrollment-service`
   - `feat/student-search`
   - `ci/github-actions`
   - `docs/architecture`
4. Make changes only on the task branch.
5. Build the solution and run the relevant tests before proposing the change as complete.
6. Review the diff and remove unrelated changes before committing.
7. Always ask the user whether they want to see the diff before creating a commit, and wait for their answer.
8. Commit changes with a concise, descriptive message.
9. Push the task branch and create a Pull Request targeting `main`.
10. Include the related GitHub Issue in the PR description, preferably using `Closes #ISSUE_NUMBER` when the PR fully resolves the Issue.
11. Summarize the implementation, tests performed, and any known limitations in the PR description.
12. Wait for review and required CI checks. Address requested changes in the same branch and PR.
13. Do not merge the PR unless explicitly authorized by the instructor or repository policy.

Never:
- Commit directly to `main`.
- Push changes directly to `main`.
- Use `git push --force` unless explicitly authorized and genuinely necessary.
- Bypass branch protection or required review and CI checks.
- Create a PR without checking which changes it contains.

If the current branch is `main`, do not edit project files. Create or switch to an appropriate task branch first. If the working tree contains uncommitted changes, inspect them and preserve them before proceeding.

## 3. Coding Conventions

### General C# Guidelines
- Follow standard C# naming conventions:
  - `PascalCase` for classes, methods, properties, and public members.
  - `camelCase` for local variables and parameters.
  - Meaningful names that describe intent.
- Use `async` and `await` for asynchronous I/O operations, including Entity Framework Core database calls.
- Pass `CancellationToken` through asynchronous operations when it fits the existing method and API design.
- Use nullable reference types and handle nullable values consistently with the existing project configuration.
- Prefer clear, readable code over clever or overly compact implementations.
- Avoid unnecessary comments that merely repeat what the code does. Comment on non-obvious decisions and business rules.

### Controllers and HTTP
- Controllers are responsible for HTTP concerns: routing, binding, response status codes, and translating application outcomes into HTTP responses.
- Keep endpoint behavior consistent with the existing API contract.
- Use appropriate HTTP status codes for successful operations, invalid requests, missing resources, and conflicts.
- Do not expose stack traces, database details, or internal exception messages to API clients.
- Avoid introducing new global error-handling mechanisms unless required by the task.

### Entity Framework Core
- Use the existing `DbContext` and entity relationships.
- Use asynchronous database methods where appropriate.
- Use `AsNoTracking()` for read-only queries when suitable.
- Avoid loading more data than the operation requires.
- Do not introduce repositories or additional data-access abstractions without a clear requirement.
- Never introduce database schema changes without considering their impact on existing data and initialization.

### Business Logic and Services
- Follow the architecture that exists in the current branch.
- Keep HTTP-specific responsibilities in controllers.
- When business logic is extracted into a service, keep the service focused on a clear responsibility and register it through dependency injection.
- Do not add unnecessary interfaces, repositories, factories, or other abstractions.
- When refactoring, preserve existing externally observable behavior unless the Issue explicitly requires a behavior change.

### Validation and Error Handling
- Validate incoming data at appropriate boundaries.
- Handle expected failure cases explicitly.
- Avoid broad exception handling that hides errors or returns misleading success responses.
- Do not swallow exceptions silently.
- Keep error responses consistent with the existing API design.

## 4. Testing Conventions

- Add or update tests when changing business logic.
- Prefer focused unit tests for isolated business rules.
- Use descriptive test names that explain the scenario and expected outcome.
- Cover successful behavior, failure cases, and relevant boundary conditions.
- Keep tests independent and deterministic.
- Do not require a production database for unit tests.
- Use the existing xUnit test project and its established conventions.
- Do not modify or remove tests merely to make the build pass.
- If a test cannot be executed, explain why rather than claiming it passed.

For enrollment-related logic, consider scenarios such as duplicate enrollment, full courses, available capacity, and missing students or courses when applicable to the implementation.

## 5. Build and Verification

Run commands from the repository root unless the project structure requires otherwise.

Restore dependencies:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build StudentManagementSystem.sln --no-restore
```

Run tests:

```bash
dotnet test StudentManagementSystem.sln --no-build
```

Run the API:

```bash
dotnet run --project src/StudentManagementSystem.Api
```

Before completing a task:
- Run the build.
- Run relevant tests.
- Inspect the final diff.
- Confirm that only intended files have changed.
- Report the actual results of commands executed.

If a command fails, investigate and report the failure. Do not claim successful verification without evidence.

## 6. Documentation

- Update the README when setup, run, or test instructions change.
- Keep architecture documentation consistent with the actual implementation.
- Update relevant documentation when a change materially affects architecture, API behavior, configuration, or development workflow.
- Do not document planned behavior as implemented behavior.
- Keep documentation changes focused on the task.

## 7. Dependencies and Scope Control

- Prefer the existing .NET and project capabilities.
- Add a NuGet dependency only when it provides clear value for the requested task.
- Avoid unrelated refactoring, formatting changes, and feature additions.
- Do not introduce new architectural patterns simply because they are popular.
- If a larger change appears necessary, explain why before expanding the scope.

## 8. Definition of Done

A task is complete only when:
- The requested behavior is implemented.
- Relevant tests have been added or updated where appropriate.
- The solution builds successfully, or any unresolved build failure is clearly reported.
- Relevant tests pass, or failures are clearly reported.
- The diff contains no unrelated changes.
- Relevant documentation has been updated.
- The changes are committed on the task branch and proposed through a Pull Request targeting `main`.
- The PR description explains the changes and verification performed.

**The Pull Request is the required path for every change to `main`. Do not bypass this workflow.**