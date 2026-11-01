using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Models;

namespace StudentManagementSystem.Api.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Student>> GetAll()
    {
        var students = _context.Students.ToList();
        return Ok(students);
    }

    [HttpGet("{id}")]
    public ActionResult<Student> GetById(int id)
    {
        var student = _context.Students.Find(id);
        if (student == null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    [HttpPost]
    public ActionResult<Student> Create(Student student)
    {
        _context.Students.Add(student);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Student updatedStudent)
    {
        var student = _context.Students.Single(s => s.Id == id);

        student.FirstName = updatedStudent.FirstName;
        student.LastName = updatedStudent.LastName;
        student.Email = updatedStudent.Email;

        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var student = _context.Students.Find(id);
        if (student == null)
        {
            return NotFound();
        }

        _context.Students.Remove(student);
        _context.SaveChanges();

        return NoContent();
    }
}
