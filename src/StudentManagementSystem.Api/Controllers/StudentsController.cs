using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Api.Models;
using StudentManagementSystem.Api.Services;

namespace StudentManagementSystem.Api.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Student>> GetAll([FromQuery] string? search = null)
    {
        var query = _context.Students.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.ToLower();
            query = query.Where(s =>
                s.FirstName.ToLower().Contains(normalizedSearch) ||
                s.LastName.ToLower().Contains(normalizedSearch) ||
                s.Email.ToLower().Contains(normalizedSearch));
        }

        var students = query.ToList();
        return Ok(students);
    }

    [HttpGet("{id}")]
    public ActionResult<Student> GetById(int id)
    {
        var student = _studentService.GetById(id);
        if (student == null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    [HttpPost]
    public ActionResult<Student> Create(Student student)
    {
        var created = _studentService.Create(student);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Student updatedStudent)
    {
        return _studentService.Update(id, updatedStudent) ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return _studentService.Delete(id) ? NoContent() : NotFound();
    }
}
