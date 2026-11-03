---
name: issue-analyzer
description: Analyze a GitHub Issue and prepare an implementation plan. Always use Plan Mode. Never implement changes.
---

# Issue Analyzer

## Mandatory Mode

**You MUST work exclusively in Plan Mode.**

- Enter or remain in Plan Mode before analyzing the issue.
- Do not switch to normal execution mode.
- Do not implement any part of the solution.
- Do not create, edit, or delete files.
- Do not run commands that modify the repository or its state.
- Do not create branches, commits, or pull requests.
- Do not install packages or modify project configuration.
- If Plan Mode is unavailable, stop and explain that the skill requires Plan Mode. Do not continue in another mode.

## Objective

Analyze a GitHub Issue and produce a clear, practical implementation plan that another developer or Claude Code session can follow.

The goal is to understand the requirement before writing code, identify the relevant parts of the existing application, and define how the solution can be implemented and verified.

## Workflow

### 1. Identify the Issue

- If the user provides a GitHub Issue number, use that issue.
- If the user provides an issue title or ID, identify the corresponding GitHub Issue.
- If no issue is specified, ask which issue should be analyzed.
- Use GitHub CLI (`gh`) to retrieve the issue when available.
- Read the actual issue title, description, acceptance criteria, labels, and state.
- If the issue cannot be retrieved or identified unambiguously, stop and explain the problem. Never invent missing issue details.

### 2. Inspect the Existing Project

Read the relevant source code and project documentation to understand the current implementation.

Identify:
- The components, classes, methods, controllers, services, and data models related to the issue.
- The current request or execution flow.
- Existing patterns and conventions that the solution should follow.
- Relevant tests and how the behavior is currently verified.
- Dependencies or previously completed issues that affect the implementation.

Use read-only inspection only. Do not modify the repository.

### 3. Analyze the Requirements

Determine:
- What problem must be solved or what capability must be added.
- What the expected behavior should be.
- Which acceptance criteria must be satisfied.
- Which edge cases and failure scenarios should be considered.
- Whether the issue depends on another issue or existing functionality.
- What risks, ambiguities, or backward-compatibility concerns exist.

If important requirements are unclear, list the questions that need answers instead of making unsupported assumptions.

### 4. Prepare the Implementation Plan

Create an ordered, actionable plan that describes:

1. **Files and components:** Which existing files are likely to require changes and why.
2. **Implementation steps:** The logical sequence of changes.
3. **Design decisions:** How the solution should fit the existing architecture and coding conventions.
4. **Testing strategy:** Which unit tests, integration tests, or manual checks are needed.
5. **Validation:** Which build and test commands should be run after implementation.
6. **Completion criteria:** How to determine whether the issue is fully resolved.

Mention new files only when they appear necessary. Do not introduce unnecessary abstractions or unrelated refactoring.

Treat file paths and implementation details as recommendations based on the code inspected, not as guaranteed facts when the available evidence is incomplete.

## Output Format

Respond in Bulgarian using the following structure:

### Issue
- GitHub Issue number and title
- Current state
- Priority and labels
- Short explanation of the requirement

### Current Implementation
- Relevant components and files
- How the current behavior works
- What needs to change and why

### Proposed Implementation Plan
Provide a numbered sequence of clear, actionable steps.

### Testing Strategy
List the expected test scenarios, including relevant success cases, validation failures, and edge cases.

### Risks and Dependencies
List relevant dependencies, potential regressions, and unanswered questions.

### Definition of Done
Provide a checklist derived from the issue's actual acceptance criteria.

### Recommendation
Briefly explain the suggested implementation approach and the most important architectural consideration.

## Strict Constraints

- Plan Mode only, without exception.
- Analysis and planning only; never implement the plan.
- Never edit files, even if a small change appears obvious.
- Never commit, push, create branches, or open pull requests.
- Never claim that tests or builds passed unless they were actually run.
- Do not expand the scope beyond the selected issue.
- Do not assume acceptance criteria that are not present in the issue.
- After presenting the plan, stop and wait for the user's instructions.
