# Next Issue

## Purpose

Select the next GitHub Issue in the predefined workshop sequence. This Skill is used during a live Claude Code workshop, so deterministic behavior is more important than dynamically prioritizing tasks.

## Instructions

1. Use GitHub CLI (`gh`) to retrieve the current Issues from the repository, including their status, title, description, acceptance criteria, and labels.
2. Follow this exact workshop sequence:
  
   1. `WS-04` — Проверка на максималния капацитет на курса
   2. `WS-05` — Връщане на HTTP 404 за несъществуващи ресурси
   3. `WS-06` — Изнасяне на логиката за записване в EnrollmentService
   4. `WS-07` — Изнасяне на логиката на StudentsController и CoursesController в services
   5. `WS-08` — Добавяне на търсене на студенти
   6. `WS-09` — Филтриране на курсове със свободни места
   7. `WS-10` — Добавяне на GitHub Actions CI pipeline
   8. `WS-11` — Добавяне на health-check endpoint
   9. `WS-12` — Добавяне на endpoint за брой записани студенти в курс

3. `WS-01` and `WS-02` and `WS-03` are intentionally excluded from automatic selection because the instructor selects them manually at the beginning of the workshop:
   - `WS-01` — Създаване и конфигуриране на CLAUDE.md
   - `WS-02` — Документиране на архитектурата за нови разработчици
   - `WS-03` - Предотвратяване на дублирани записвания за курс 

4. Select the first Issue in the predefined sequence that is still open.

5. Never skip an earlier open Issue to select a later one, even if the later Issue has a higher priority or appears easier to implement.

6. If an Issue is closed, skip it and continue to the next Issue in the sequence.

7. If an expected Issue cannot be found in the repository, stop and report the missing Issue. Do not silently substitute another task.

8. If all Issues in the sequence are closed, report that there are no remaining Issues in the predefined workshop sequence. Do not invent another task.

9. Explain the selected Issue in Bulgarian, including:
    - The workshop Issue ID (`WS-XX`) and actual GitHub Issue number.
    - The exact title and description from GitHub.
    - The acceptance criteria defined in the Issue.
    - Its priority and labels, if available.
    - Why it is the next task according to the predefined sequence.
    - Any relevant dependency or prerequisite.

10. Use the actual GitHub Issue content as the source of truth for its description and acceptance criteria. Do not invent missing requirements. If acceptance criteria are not explicitly provided, state that clearly.

11. Do not edit files, create branches, modify Issues, commit changes, open pull requests, or implement the selected task. This Skill only identifies and explains the next Issue.

## Error handling

- If GitHub CLI (`gh`) is unavailable, report the problem.
- If GitHub authentication fails, report the problem.
- If the repository cannot be identified, report the problem and ask the instructor to resolve the repository context.
- If retrieving the Issues fails, report the error.
- Never pretend to have checked the Issues or their status if the commands failed.
- Do not fall back to guessing based on previous conversations or cached Issue statuses.

## Output

Clearly announce the selected Issue, then explain its requirements and why it is next.

End by waiting for the instructor to decide when implementation should begin. Do not start implementation automatically.